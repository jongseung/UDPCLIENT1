using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

/* UDP 클라이언트 통신 기본 예제
 *  기능
 *  UDP 서버에 문자열을 송신
 * >MemoryStream.BinaryFormatter를 활용한 변수/객체 전달
 */
namespace UDPCLIENT
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient 객체 생성 - 데이터를 먼저 보내는 송신자측은 포트번호 임시 발급
            UdpClient client = new UdpClient(10000); //PORT번호 고정
            //서버의 IP/PORT를 저장
            for (int i = 1; i < 100; i++)
            {
                string ip_b = "114.70.60." + i;
                if (ip_b != "114.70.60.88" && ip_b != "114.70.60.84")
                {
                    //string ip_b = "114.70.60.87";

                    IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse(ip_b), 11000);

                    //데이터 통신
                    string data = "\n" +
         " ⊂_ヽ\n" +
         "    ＼＼ Λ＿Λ\n" +
         "     ＼( ‘ㅅ' ) 두둠칫\n" +
         "       >　⌒ヽ\n" +
         "      / 　 へ＼\n" +
         "     /　　/　＼＼\n" +
         "     ﾚ　ノ　　 ヽ_つ\n" +
         "    /　/두둠칫\n" +
         "   /　/|\n" +
         "  (　(ヽ\n" +
         "  |　|、＼\n" +
         "  | 丿 ＼ ⌒)\n" +
         "  | |　　) /\n" +
         "`ノ )　　Lﾉ \n" +
         "안녕하세요 ! 두둠칫입니다";


                    //UDP 통신으로 이진테이터를 송신하려면 변수값을 MemoryStream에 저장
                    // -> MemoryStream에 저장된 값을 byte[]로 변환한 뒤 Send메소드로 송신
                    MemoryStream stream = new MemoryStream();                                   //메모리에 값을 순차적으로 저장 MemoryStream
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, data);                                          // 변수 / 객체 - > 메모리스트림에 저장

                    //Stream 객체에 저장된 데이터를 byte[] 추출
                    byte[] byte_data = stream.ToArray();
                    //stream.Close();
                    //입력한 문자열을 byte배열로 변경
                    //byte[] byte_data = Encoding.UTF8.GetBytes(data);
                    //데이터를 수신받는 프로그램이 없더라도 비연결지향 프롵초콜인 UDP는 예외가 발생하지 않음
                    client.Send(byte_data, byte_data.Length, des_ip);

                    //기존의 변수를 할용해 데이터 수신 코드 구현

                    //수신자측에서 보내는 데이터를 수신
                    //수신된 데이터를 byte[] 배열에 저장
                    byte_data = client.Receive(ref des_ip);                 //Receive메소드를 사용하기 위해서 위에서 포트번호를 고정해야 한다.
                    
                    //1. MemoryStream 객체에 저장된 값을 초기화 및 byte[]값을 Write 메소드로 입력
                    // MemotyStream.SetLenght(숫자) : 입력한 숫자만큼의 데이터를 저장할수있또록 저장곤간을 수정하는 메소드, 
                    // 0을 입력하면 저장된 모든 데이터를 지움
                    stream.SetLength(0);                     //stream객체에 저장된 데이터를 지움
                                                    //2. MemoryStream 객체 생성 및 byte[]값을 Write 메소드로 입력
                                                    // -> MemoryStream stream1 = new MemoryStream(); 종승  -> 새로운 객체 생성, 최적화에는 좋지 못하다.
                    stream.Write(byte_data, 0, byte_data.Length);

                    // 커서 위치를 데이터 맨앞으로 이동
                    stream.Seek(0, SeekOrigin.Begin);                   //커서를 앞으로 이동시켜야 에러가 나지 않는다. (데이터의 처음위치로 이동) 
                                                                       //처음위치로 가야하는 이유는 Deserialize 메소드가 앞에서 부터 데이터를 불러오기 때문에

                    // 이진데이터 => 문자열 변환 및 출력
                    data = (string)formatter.Deserialize(stream);

                    Console.WriteLine(data);
                    stream.Close();
                }
                //UdpClient 객체 종료
            }
            client.Close();
        }
    }
}