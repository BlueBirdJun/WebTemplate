using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Enums
{
    public enum ConnectionDBNames
    {
        Log, Temp
    }

    public enum HttpMethods
    {
        POST, GET, PUT, DELETE, PATCH, DEBUG, POST2
    }

    /// <summary>
    /// http 상태코드
    /// </summary>
    public enum HttpStatusCode
    {
        /// <summary>
        /// 괜찮아요
        /// </summary>
        H200 = 200,
        /// <summary>
        /// 새로만들었어요
        /// </summary>
        H201 = 201,
        /// <summary>
        /// 서버 내부 에러 
        /// 보통 Production server를 실행했을때 에러가 발생하면 500이 발생합니다. 침착하게 서버 로그를 봅시다.
        /// </summary>
        H500 = 500,
        /// <summary>
        /// 서비스 지원 불가 
        /// 가령 웹 서버는 살아있지만 DB서버가 죽었다던가 하는 경우엔 정상적인 서비스 제공이 불가능합니다. 그런 경우에 발생시키면 좋습니다.
        /// </summary>
        H503 = 503,
        /// <summary>
        /// 요청이 이상해요 - 400 Bad Request
        /// 가령 생년월일 입력란에 2018-02-31 같은게 온 경우를 말합니다. 그 외에도 request body가 JSON이 예상되었는데 엉뚱한게 온 경우에도 400을 내보내면 됩니다.
        /// </summary>
        H400 = 400,
        /// <summary>
        /// 인증은 통과하는데 이 데이타를 줄 권한이 없다네
        /// 너에게 허락해줄 리소스는 따위는 없다네
        /// </summary>
        H401 = 401,
        /// <summary>
        /// 애초에 권한이 없다네        
        /// </summary>
        H403 = 403,
        /// <summary>
        /// 해당 Method는 안 돼요 - 405 Method Not Allowed
        /// 가령 Read-only endpoint에 POST 등의 변경 요청이 온 경우에 내보내면 됩니다.
        /// </summary>
        H405 = 405,
        /// <summary>
        /// 서비스 지원 불가 
        /// 가령 웹 서버는 살아있지만 DB서버가 죽었다던가 하는 경우엔 정상적인 서비스 제공이 불가능합니다. 그런 경우에 발생시키면 좋습니다.
        /// </summary>
        H404 = 404,
        /// <summary>
        /// 사용할수 없는 리소스        
        /// </summary>
        H410 = 410,
        /// <summary>
        /// 요청 너무 많이 하셨어요 - 429 Too Many Requests
        /// API에 rate limit 등이 있는데 요청이 제한량을 넘어버린 경우 사용합니다. 필수는 아니지만 Retry-After 헤더로 언제 이후로 다시 해보라던가 하는 힌트를 제공할 수 있습니다.
        /// </summary>
        H429 = 429,
        /// <summary>
        /// 파라미터 에러
        /// </summary>
        H422 = 422,
        /// <summary>
        /// 이사갔어요 - 301 Moved Permanently
        /// 접속한 주소가 영영 다른 위치로 옮겨진 경우에 사용합니다. Location 헤더로 다른 위치를 알려줘야 합니다. 브라우저에서도 새 위치를 기억하고, 검색엔진들도 새 위치로 URL을 변경합니다.
        /// </summary>
        H301 = 301,
        /// <summary>
        /// 바뀐게 없어요 - 304 Not Modified
        /// 200을 받아간 뒤로 바뀐 부분이 없는데 또 요청이 온 경우에 반환합니다. 브라우저에서 Cache된 내용을 사용하게 됩니다.
        /// </summary>
        H304 = 304,
        /// <summary>
        /// 잠깐만 여기로 가주세요 - 302 Found
        /// 완전히 옮겨가버린 301과 달리 302의 경우는 임시 이전입니다. Location 헤더로 다른 위치를 알려줘야 합니다.
        /// </summary>
        H302 = 302
    }

}
