using UnityEngine.Networking;

namespace Project.Util
{
    public static class HttpUtil
    {
        public static bool HasRequestError(UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError
                || request.result == UnityWebRequest.Result.DataProcessingError;
        }
    }
}
