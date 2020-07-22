/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID ARROWAIR_EVENT = 1876790197U;
        static const AkUniqueID ARROWFALLGROUND_EVENT = 1601986307U;
        static const AkUniqueID ARROWHIT_EVENT = 2659507634U;
        static const AkUniqueID ARROWNOCK_EVENT = 3210970906U;
        static const AkUniqueID ARROWPULLBACK_EVENT = 926319793U;
        static const AkUniqueID ARROWREALESE_EVENT = 544038300U;
        static const AkUniqueID ARROWSPAWN_EVENT = 3984451668U;
        static const AkUniqueID BGM_EVENT = 1799075776U;
        static const AkUniqueID BOWGRIP = 3576952471U;
        static const AkUniqueID INTROBGM = 2489435561U;
        static const AkUniqueID MUSEUM = 1362148849U;
        static const AkUniqueID SCENELOAD = 2610981307U;
        static const AkUniqueID SIGISOUND = 4266279214U;
        static const AkUniqueID TARGETMOVE = 2703374149U;
    } // namespace EVENTS

    namespace SWITCHES
    {
        namespace SW_ARROWPROGRESS
        {
            static const AkUniqueID GROUP = 210977264U;

            namespace SWITCH
            {
                static const AkUniqueID AIR = 1050421051U;
                static const AkUniqueID PULLBACK = 830775945U;
                static const AkUniqueID REALEASE = 3095539507U;
            } // namespace SWITCH
        } // namespace SW_ARROWPROGRESS

        namespace SW_ARROWRESULT
        {
            static const AkUniqueID GROUP = 3475700082U;

            namespace SWITCH
            {
                static const AkUniqueID FALL = 2512384458U;
                static const AkUniqueID HIT = 1116398592U;
            } // namespace SWITCH
        } // namespace SW_ARROWRESULT

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID BGM_PARAMETER = 3805275197U;
        static const AkUniqueID PR_PULLBACK = 891400966U;
        static const AkUniqueID PR_SHOTBOW = 877766230U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID ADDSB = 2570592105U;
        static const AkUniqueID INTRO = 1125500713U;
        static const AkUniqueID LEFTDESK = 742198245U;
        static const AkUniqueID SB_ARROWRESULT = 1501255583U;
        static const AkUniqueID SB_ARROWSOUND = 3256071817U;
        static const AkUniqueID SB_BGMEVENT = 3231291807U;
        static const AkUniqueID SB_MUSEUM = 2719379385U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
