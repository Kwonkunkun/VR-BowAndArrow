using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class DemoCameraController : MonoBehaviour {

	[Header("Track Objects")]
	public Transform pivotObject;
	public Transform cameraObject;
	public Transform lightObject;

	[Header("Wireframe Setup")]
	public Material wireframeShader;
	public Renderer[] excludeRenderers;


	[Header("Controller Attributes")]
	public bool reverseVAxis = true;
	public bool reverseHAxis = false;
	public float minZoomVal = -1.97f;
	public float maxZoomVal = -0.2f;
	

	[Header("Scene Objects")]
	public bool HideText = true;
	public Transform[] SlotObjects;
	public Transform[] TextObjects;


	public int currentSlot = 2;
	private int saveSlot = 2;
	private float slotPos;
	private float slotPosX;
	private float slotTimer;

	private bool keydown0;
	private bool keydown1;
	//private bool keydown2;
	private float vAxis;
	private float hAxis;
	private float wheelAxis;
	private float zoomLevel;
	private float zoomTimer;
	private float zoomTarget;
	private float zoomVal;
	private bool zoomStart = false;
	private float WhTarget;
	private float transTimer;
	private float oldTransTimer;
	private float canMoveTimer;

	private bool wMode;


	void Start(){

		//set max value
		if (currentSlot > SlotObjects.Length-1) currentSlot = SlotObjects.Length-1;

		//set starting zoom level and position
		zoomLevel = 0.25f;
		zoomVal = -1.211f;
		slotPos = SlotObjects[currentSlot].localPosition.z;
		slotPosX = SlotObjects[currentSlot].localPosition.x;

		//Initialize all text labels (remove)
		if (HideText){
			for (int sNum = 0; sNum < TextObjects.Length; sNum++){
				if (TextObjects[sNum] != null){
				    foreach (Transform oldChild in TextObjects[sNum]){
				    	Text cText = oldChild.GetComponent<Text>();
						cText.color = new Color(cText.color.r,cText.color.g,cText.color.b,0f);
				    }
				}
			}
		}

		//Initialize Wireframe shader on scene objects
		if (wireframeShader != null && SystemInfo.graphicsShaderLevel >= 40){

			//assign new materials array
	        Renderer[] rendObjs = FindObjectsOfType(typeof(Renderer)) as Renderer[];
	        foreach (Renderer objRenderer in rendObjs) {

	        	if(!excludeRenderers.Contains(objRenderer)){
	        		objRenderer.sharedMaterials = new Material[] {objRenderer.sharedMaterial, wireframeShader};
	        	}
	        }
		}

	}



	void LateUpdate () {
		if (pivotObject != null && cameraObject != null){

			//GET KEY VALUES --------------------------------------------------
			keydown0 = Input.GetMouseButton(0);
			keydown1 = Input.GetMouseButton(1);
			//keydown2 = Input.GetMouseButton(2);
			wheelAxis = Input.GetAxis("Mouse ScrollWheel");

			vAxis = Input.GetAxis("Mouse Y");
			hAxis = Input.GetAxis("Mouse X");

			//Reverse Axis
			vAxis = vAxis * (reverseVAxis ? -1f : 1f);
			hAxis = hAxis * (reverseHAxis ? -1f : 1f);

			//Calculate Current Zoom Level
			zoomLevel = (cameraObject.localPosition.z - minZoomVal) / (maxZoomVal - minZoomVal);


			//SET CURRENT POSITION ---------------------------------------------
			canMoveTimer += Time.deltaTime;
			if (canMoveTimer > 0.5f){
				if (Input.GetKeyDown("2")){
						canMoveTimer = 0f;
						saveSlot = currentSlot;
						currentSlot = currentSlot + 1;
						slotTimer = 0.0f;
						transTimer = 0.0f;
						oldTransTimer = 0.0f;
				}
				if (Input.GetKeyDown("1")){
						canMoveTimer = 0f;
						saveSlot = currentSlot;
						currentSlot = currentSlot - 1;
						slotTimer = 0.0f;
						transTimer = 0.0f;
						oldTransTimer = 0.0f;
				}
				if (currentSlot < 0) currentSlot = 0;
				if (currentSlot > SlotObjects.Length-1) currentSlot = SlotObjects.Length-1;
			}

			//PIVOT MOUSE ----------------------------------------------------
			if (keydown0){
				pivotObject.eulerAngles = new Vector3(
					pivotObject.eulerAngles.x + vAxis,
					pivotObject.eulerAngles.y + hAxis,
					pivotObject.eulerAngles.z
					);

				//Clamp Vertical Pivot Values
				if (pivotObject.eulerAngles.x < 8f){
					pivotObject.eulerAngles = new Vector3(8f, pivotObject.eulerAngles.y, pivotObject.eulerAngles.z);
				}
				if (pivotObject.eulerAngles.x > 79f){
					pivotObject.eulerAngles = new Vector3(79f, pivotObject.eulerAngles.y, pivotObject.eulerAngles.z);
				}
			}


			//ZOOM MOUSE -------------------------------------------------------
			if (wheelAxis != 0f){
				//Reset Zoom Timer
				WhTarget = wheelAxis;
				zoomTimer = 0f;
				zoomTarget = cameraObject.localPosition.z + (WhTarget * Mathf.Lerp(6.0f, 1.0f, zoomLevel));
				zoomStart = true;
			}

			//Zoom Camera
			zoomTimer += Time.deltaTime;
			if (zoomTimer < 2.0f && zoomStart){
				zoomVal = Mathf.SmoothStep(zoomVal, zoomTarget, zoomTimer);
				cameraObject.localPosition = new Vector3(0f, 0f,zoomVal);
			}

			//Clamp Zoom values
			if (cameraObject.localPosition.z > maxZoomVal){
				cameraObject.localPosition = new Vector3(0f, 0f, maxZoomVal);
			}
			if (cameraObject.localPosition.z < minZoomVal){
				cameraObject.localPosition = new Vector3(0f, 0f, minZoomVal);
			}

			//move pivot based on zoom level and Slot Position
			slotTimer += Time.deltaTime;
			slotPos = Mathf.SmoothStep(slotPos,SlotObjects[currentSlot].localPosition.z,slotTimer);
			slotPosX = Mathf.SmoothStep(slotPosX,SlotObjects[currentSlot].localPosition.x,slotTimer);

			//pivotObject.localPosition = new Vector3(
			//	Mathf.Lerp(-0.1f, 0.2f, zoomLevel),
			//	pivotObject.localPosition.y,
			//	Mathf.Lerp(slotPos+0.189f, slotPos, zoomLevel)
			//	);
			
			pivotObject.localPosition = new Vector3(
				Mathf.Lerp(slotPosX - 0.3f, slotPosX, zoomLevel),
				pivotObject.localPosition.y,
				Mathf.Lerp(slotPos+0.189f, slotPos, zoomLevel)
				);

			

			//MOVE SCENE LIGHT -------------------------------------------------
			if (lightObject != null && keydown1){
				//Set Light Rotaion
				lightObject.eulerAngles = new Vector3(
					lightObject.eulerAngles.x + (vAxis * -1.5f),
					lightObject.eulerAngles.y + (hAxis * 2.0f),
					lightObject.eulerAngles.z
					);

				//Clamp Vertical Pivot Values
				if (lightObject.eulerAngles.x < 2f){
					lightObject.eulerAngles = new Vector3(2f, lightObject.eulerAngles.y, lightObject.eulerAngles.z);
				}
				if (lightObject.eulerAngles.x > 90f){
					lightObject.eulerAngles = new Vector3(2f, lightObject.eulerAngles.y, lightObject.eulerAngles.z);
				}
				if (lightObject.eulerAngles.x > 79f){
					lightObject.eulerAngles = new Vector3(79f, lightObject.eulerAngles.y, lightObject.eulerAngles.z);
				}



			}


			//HANDLE TEXT FADING ------------------------------------------------
			if (HideText){
				//Remove old Text
				oldTransTimer += (Time.deltaTime * 1.5f);
			    foreach (Transform oldChild in TextObjects[saveSlot]){
			    	Text ocText = oldChild.GetComponent<Text>();
					ocText.color = new Color(ocText.color.r,ocText.color.g,ocText.color.b,Mathf.SmoothStep(1f,0f,oldTransTimer));
			    }


				//Highlight new Text
				transTimer += (Time.deltaTime * 1.75f);
			    foreach (Transform child in TextObjects[currentSlot]){
			    	Text cText = child.GetComponent<Text>();
					cText.color = new Color(cText.color.r,cText.color.g,cText.color.b,Mathf.SmoothStep(0f,1f,transTimer));
			    }
			}



			//HANDLE WIREFRAME MODE ----------------------------------------------------
			if (wireframeShader != null && SystemInfo.graphicsShaderLevel >= 40){
				if (Input.GetKeyDown("4")) wMode = !wMode;
				wireframeShader.SetFloat("showWireframe", wMode ? 1f : 0f );
			}



		}
	}





}
