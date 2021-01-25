using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Math_Quiz
{
    public partial class Form1 : Form
    {
        const int MAX_SIZE = 8; //최대 생성 개수 (퀴즈 라벨 수에 따름)
        const int MAX_RAND_NUM_GEN_SIZE = 40; //최대 난수 생성 범위
        const int TIMER_START_TIME = 30; //타이머 시작 시간
        class Quiz_Data
        {
            #region public:
            public Quiz_Data()
            {
                this.quiz_label = null;
                this.quiz_number = null;
                this.is_quiz_label_initialized = false;
            }
            public Quiz_Data(int size) //size만큼 생성
            {
                this.quiz_label = new Label[size];
                this.quiz_number = new int?[size];
                this.is_quiz_label_initialized = false;

                for(int i=0;i<size;i++)
                {
                    this.quiz_label[i] = null;
                    this.quiz_number[i] = null;
                }
            }
            public Label[] Quiz_label
            {
                get { return this.quiz_label; }
                set { this.quiz_label = value; }
            }

            public int?[] Quiz_number
            {
                get { return this.quiz_number; }
                set { this.quiz_number = value; }
            }

            public bool Is_quiz_label_initialized
            {
                get { return this.is_quiz_label_initialized; }
                set { this.is_quiz_label_initialized = value; }
            }
            #endregion

            #region private:
            private Label[] quiz_label; //퀴즈 라벨 배열
            private int?[] quiz_number; //해당 라벨의 생성 된 숫자 배열
            private bool is_quiz_label_initialized; //퀴즈 라벨 초기 할당 여부
            #endregion
        }

        Random rand;
        Quiz_Data quiz_data = null;
        int time_left; //남은 시간

        public void start_quiz() //퀴즈 시작
        {
            rand = new Random();
                
            if (quiz_data == null)
            {
                quiz_data = new Quiz_Data(MAX_SIZE);

                quiz_data.Quiz_label[0] = plusLeftLabel;
                quiz_data.Quiz_label[1] = plusRightLabel;
                quiz_data.Quiz_label[2] = minusLeftLabel;
                quiz_data.Quiz_label[3] = minusRightLabel;
                quiz_data.Quiz_label[4] = timesLeftLabel;
                quiz_data.Quiz_label[5] = timesRightLabel;
                quiz_data.Quiz_label[6] = dividedLeftLabel;
                quiz_data.Quiz_label[7] = dividedRightLabel;
                quiz_data.Is_quiz_label_initialized = true; //퀴즈 라벨 초기 할당 완료 알림
            }

            if (!quiz_data.Is_quiz_label_initialized)
            {
                MessageBox.Show("오류 : 할당되지 않은 퀴즈 라벨");
                this.Close();
            }

            for (int i = 0; i < MAX_SIZE; i++)
            {
                quiz_data.Quiz_number[i] = rand.Next(MAX_RAND_NUM_GEN_SIZE);
                quiz_data.Quiz_label[i].Text = quiz_data.Quiz_number[i].ToString(); //생성 된 숫자를 라벨에 표시
            }

            //타이머 시작
            time_left = TIMER_START_TIME;
            timeLabel.Text = time_left + " seconds";
            timer.Start();
        }

        public bool chk_answer() //정답 확인
        {
            if ((decimal)(quiz_data.Quiz_number[0] + quiz_data.Quiz_number[1]) == sum.Value &&
               (decimal)(quiz_data.Quiz_number[2] - quiz_data.Quiz_number[3]) == difference.Value &&
               (decimal)(quiz_data.Quiz_number[4] * quiz_data.Quiz_number[5]) == product.Value &&
               (decimal)(quiz_data.Quiz_number[6] / quiz_data.Quiz_number[7]) == quotient.Value)
                return true;

            return false;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            start_quiz();
        }

        private void timer_Tick(object sender, EventArgs e) //타이머에 대한 이벤트 처리
        {
            if(chk_answer()) //모든 정답을 맞출 시
            {
                timer.Stop();
                MessageBox.Show("You got all the answers right!", "Congratulations!");
            }
            else if(time_left > 0)
            {
                time_left -= 1;
                timeLabel.Text = time_left + " seconds";
            }
            else
            {
                timer.Stop();
                timeLabel.Text = "Time's up!";
                MessageBox.Show("You didn't finish in time.", "Sorry!");
                
                //정답 표시
                sum.Value = (decimal)(quiz_data.Quiz_number[0] + quiz_data.Quiz_number[1]);
                difference.Value = (decimal)(quiz_data.Quiz_number[2] - quiz_data.Quiz_number[3]);
                product.Value = (decimal)(quiz_data.Quiz_number[4] * quiz_data.Quiz_number[5]);
                quotient.Value = (decimal)(quiz_data.Quiz_number[6] / quiz_data.Quiz_number[7]);
            }

            if (time_left <= 5)
                timeLabel.BackColor = Color.Red;
            else 
                timeLabel.BackColor = Color.White;
        }

        private void answer_Enter(object sender, EventArgs e) //NumericUpDown 컨트롤에 대한 Enter 이벤트 처리
        {
            NumericUpDown answer_box = sender as NumericUpDown; //전송자 개체인 NumbericUpDown 컨트롤임을 명시적으로 지정

            if (answer_box != null)
                answer_box.Select(0, answer_box.Value.ToString().Length); //기존 내용 덮어쓰기 (03 입력 시 => 3으로 변경)
        }
    }
}
