using System;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            byte[] orgBytes = Convert.FromBase64String("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");

            // 바이트들을 다시 유니코드 문자열로
            string orgStr = Encoding.Unicode.GetString(orgBytes);
            Console.WriteLine(orgStr);
            
            //console.log('payload: ', encodedPayload);


        }
    }
}
