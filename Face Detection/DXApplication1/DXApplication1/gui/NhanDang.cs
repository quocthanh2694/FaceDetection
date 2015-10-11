using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;

namespace DXApplication1.gui
{
    public partial class NhanDang : Form
    {
        public int HDfaces = 1;
        public int mauso = 1;
        public NhanDang()
        {
            InitializeComponent();

            InitGrid();
            try
            {

                face = new HaarCascade("Nariz_face.xml");
                eye = new HaarCascade("eye.xml");
                nose = new HaarCascade("Nariz_nuevo_20stages(nose).xml");
                mouth = new HaarCascade("Nariz_mouth.xml");
            }
            catch (Exception i) { MessageBox.Show(i.ToString()); }
            try
            {
                //Load of previus trainned faces and labels for each image
                // LoadImageAndnames();
            }
            catch (Exception es)
            {
                MessageBox.Show("Nothing in database, please add at least a face(Simply train the prototype with the AddFace Button)" + es.ToString(), "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
        #region load image and name
        private void LoadImageAndnames()
        {
            //       string Labelsinfo = File.ReadAllText(Application.StartupPath + "/Faces/TrainedLabels.txt");
            //       string[] Labels = Labelsinfo.Split('%');

            //       NumLabels = Convert.ToInt16(Labels[0]);
            tong = 0;
            //       ContTrain = NumLabels;
            string LoadFaces = "";
            trainingImages = null;
            trainingImages = new List<Image<Gray, byte>>();
            labels = null;
            labels = new List<string>();

            // FileInfo[] Archivesnose = directory.GetFiles("nose*.bmp");
            foreach (FileInfo fileifo in Archives)
            {
                string[] name = fileifo.Name.Split('.');
                LoadFaces = name[0];

                if (LoadFaces.Substring(0, 4) == "face")
                {
                    tong++;
                    tf = Convert.ToInt16(LoadFaces.Substring(4));
                    int k = tf - Convert.ToInt16(tf.ToString().Substring(tf.ToString().Length - 1));

                    trainingImages.Add(new Image<Gray, byte>(directory + LoadFaces + ".bmp"));
                    labels.Add(kketnoi.lay1dong("select tensv from sinhvien sv, hinh h where sv.mssv=h.mssv and hinh='" + k + "'"));
                    Bitmap bmp = new Bitmap(directory + LoadFaces + ".bmp");
                    Bitmap tamnewsize = new Bitmap(bmp, newsizegb);
                    gabor(tamnewsize);
                    matrix1s.Add(x);
                    //  MessageBox.Show(directory + LoadFaces + ".bmp" + "\n" + k);
                    //  pictureBox_thu.Image = Image.FromFile(directory + LoadFaces);
                    //    Bitmap bmp = new Bitmap(Image.FromFile(directory + LoadFaces));
                    //  imb_thu.Image = new Image<Bgr, byte>(bmp);
                }
                else

                    if (LoadFaces.Substring(0, 4) == "nose")
                {
                    tf = Convert.ToInt16(LoadFaces.Substring(4, 1));
                    trainingImagenose.Add(new Image<Gray, byte>(directory + LoadFaces));
                    //   labelsnose.Add(kketnoi.lay1dong("select ten from sinhvien where hinh=" + tf + "or hinh=" + (tf - 1) + " or hinh=" + (tf - 2) + ""));
                }
                else
                        if (LoadFaces.Substring(0, 4) == "eyeL")
                {

                    tf = Convert.ToInt16(LoadFaces.Substring(4, 1));
                    trainingImageneyeL.Add(new Image<Gray, byte>(directory + LoadFaces));
                    //    labelseyeL.Add(kketnoi.lay1dong("select ten from sinhvien where hinh=" + tf + "or hinh=" + (tf - 1) + " or hinh=" + (tf - 2) + ""));
                }
                else
                            if (LoadFaces.Substring(0, 4) == "eyeR")
                {
                    tf = Convert.ToInt16(LoadFaces.Substring(4, 1));
                    trainingImageneyeR.Add(new Image<Gray, byte>(directory + LoadFaces));
                    //     labelseyeR.Add(kketnoi.lay1dong("select ten from sinhvien where hinh=" + tf + "or hinh=" + (tf - 1) + " or hinh=" + (tf - 2) + ""));
                }
                else
                //  if (LoadFaces.Substring(0, 4) == "mouth")
                {

                    tf = Convert.ToInt16(LoadFaces.Substring(5, 1));
                    trainingImagemouth.Add(new Image<Gray, byte>(directory + LoadFaces));
                    //     labelsmouth.Add(kketnoi.lay1dong("select ten from sinhvien where hinh=" + tf + "or hinh=" + (tf - 1) + " or hinh=" + (tf - 2) + ""));
                }





            }
            ContTrain = tf;

        }
        #endregion 

        private void loaddanhsachsv()
        {

            labels = null;
            matrix1s = null;
            labels = new List<string>();
            matrix1s = new List<Matrix1>();

            int len = newsizegb.Width; //MessageBox.Show("new size gb with" + len.ToString());
            dt = kketnoi.laydl("select * from sinhvien where malop ='" + loptxt.Text + "'");
            int indexmt; string dtt2 = "";//============================
            for (int tbd = 0; tbd < dt.Rows.Count; tbd++)
            {

                string[] mangmatran = dt.Rows[tbd]["Hinh"].ToString().Split('@');

                for (int i = 0; i < mangmatran.Length - 1; i++)
                {
                    string[] mangmatran1 = mangmatran[i].Split('.');
                    // string s = "";
                    x = new Matrix1(len, len);
                    indexmt = 0;
                    for (int j = 0; j < len; j++)
                        for (int k = 0; k < len; k++)
                        {
                            x[j, k] = Convert.ToInt16(mangmatran1[indexmt]);//mangmatran[i][indexmt] - 48;
                            dtt2 += x[j, k];
                            indexmt++;
                            //s += x[j, k];
                        }
                    //   MessageBox.Show(s);
                    labels.Add(dt.Rows[tbd]["Tensv"].ToString());

                    matrix1s.Add(x);

                }
                // dtt2 += x.ToString();//=======================

            }
            string fileName = Application.StartupPath + "\\dtt2.txt";
            StreamWriter sw = new StreamWriter(fileName, false);
            sw.WriteLine(dtt2);
            sw.Close();

            // gridControl1.DataSource = dt;
        }
        ketnoi kketnoi = new ketnoi();
        DataTable dt = new DataTable();
        string[] tenanh = new string[3];
        List<string> imagearray = new List<string>();
        private int dem = 0, k = 0;

        string name1 = "", stt = "", directorypath = "";
        int demhinh, tf, tong, SoNguoi = 0;
        int yM = 0, xM = 0;
        double sf = 0;

        DirectoryInfo directory;
        FileInfo[] Archives;

        Rectangle rt = new Rectangle();
        Rectangle rte = new Rectangle();
        Rectangle rtm = new Rectangle();
        Rectangle rtn = new Rectangle();

        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade eye, nose, face, mouth;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> resultface, resulteyeL, resulteyeR, resultnose, resultmouth, TrainedFace = null;
        Image<Gray, byte> gray = null;
        Image<Gray, byte> grayf = null;
        Image<Gray, byte> graye = null;
        Image<Gray, byte> grayfm = null;
        Image<Gray, byte> graytam = null;
        Image<Gray, byte> grayfn = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImagenose = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImageneyeL = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImageneyeR = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImagemouth = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();

        List<Image<Gray, byte>> tface = new List<Image<Gray, byte>>();

        List<string> tten = new List<string>();



        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;



        //BindingList<Person> gridDataList = new BindingList<Person>();
        void InitGrid()
        {

        }
        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                //  NamePersons.Add("");



                //Get the current frame form capture device
                //currentFrame = grabber.QueryFrame().Resize(600, 440, INTER.CV_INTER_CUBIC);
                currentFrame = grabber.QueryFrame().Resize(600, 480, INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
              face,
              1.2,
              10,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
              new Size(60, 60));

                SoNguoi = 0;
                //Action for each element detected
                //  gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp f in facesDetected[0])
                {

                    //////////xu ly face
                    t = t + 1;
                    resultface = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    sf = f.rect.Width / 100.0;

                    //draw the face detected in the 0th (gray) channel with blue color
                    currentFrame.Draw(f.rect, new Bgr(Color.Green), 2);

                    grayf = resultface.Resize(30, 30, INTER.CV_INTER_CUBIC);
                    Bitmap tam = grayf.ToBitmap();
                    //Bitmap tamnewsize = new Bitmap(tam, newsizegb);
                    matrixtam = PCA.image_2_matrix(tam);
                    matrixtam = Radon1.ApdungRadon(matrixtam);//PCA.apDungWaveletGabors(matrixtam, 0, 1.56, 1);
                                                              /////////////////////////////////////////

                    #region detect eye, nose, mouth

                    //phat hien mat
                    //eye detect        
                    grayf = resultface;


                    rte.X = 0; rte.Y = 15;
                    rte.Width = 100;
                    rte.Height = 40;
                    graye = grayf.Copy(rte).Convert<Gray, byte>();

                    MCvAvgComp[][] eyesDetected = graye.DetectHaarCascade(
                                                  eye,
                                                 1.02,
                                                  5,
                                                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                                  new Size(20, 20));

                    int k = 0;
                    foreach (MCvAvgComp es in eyesDetected[0])
                    {

                        rt.X = (int)(sf * es.rect.X) + f.rect.X;
                        rt.Y = (int)(sf * (es.rect.Y + 15)) + (int)((f.rect.Y));
                        rt.Width = (int)(26 * sf);
                        rt.Height = (int)(26 * sf);


                        currentFrame.Draw(rt, new Bgr(Color.Yellow), 2);

                        rt.X = es.rect.X; rt.Y = es.rect.Y + 15;
                        rt.Width = 23;
                        rt.Height = 23;
                        graytam = grayf.Copy(rt).Convert<Gray, byte>();
                        if (rt.X > 50)
                        {
                            this.ibe1.Image = graytam;
                            resulteyeR = graytam;
                        }
                        else
                        if (rt.X <= 50)
                        {
                            this.ibe2.Image = graytam;
                            resulteyeL = graytam;
                        }

                        k++;
                        if (k == 2) break;

                    }

                    ////////////////////////////////////////////////////cat mouth tren grayface
                    rtm.X = 0; rtm.Y = 60;
                    rtm.Width = 100;
                    rtm.Height = 40;
                    grayfm = grayf.Copy(rtm).Convert<Gray, byte>();

                    ////////////////////////////////////////


                    //mouth detector
                    MCvAvgComp[][] mouthsDetected = grayfm.DetectHaarCascade(
                mouth,
                1.1,
                5,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                new Size(20, 20));
                    //  Console.WriteLine(mouthsDetected.Length);

                    foreach (MCvAvgComp m in mouthsDetected[0])
                    {

                        rt.X = (int)(m.rect.X * sf) - 2 + (int)(f.rect.X);
                        rt.Y = (int)((60 + m.rect.Y) * sf) + (int)(f.rect.Y);

                        rt.Width = (int)(40 * sf);
                        rt.Height = (int)(20 * sf);
                        currentFrame.Draw(rt, new Bgr(Color.Black), 2);
                        rt.X = m.rect.X; rt.Y = m.rect.Y + 60;
                        rt.Width = 40;
                        rt.Height = 20;
                        graytam = grayf.Copy(rt).Convert<Gray, byte>();
                        this.ibm.Image = graytam;
                        resultmouth = graytam;
                        break;
                    }

                    ////////////////////////////////////////////////////

                    ////////////////////////////////////////////////////cat mũi tren face

                    rtn.X = 0;
                    rtn.Y = 30;
                    rtn.Width = 100;
                    rtn.Height = 50;
                    grayfn = grayf.Copy(rtn).Convert<Gray, byte>();

                    //nose detect
                    MCvAvgComp[][] nosesDetected = grayfn.DetectHaarCascade(
              nose,
             1.1,
              5,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
              new Size(20, 20));
                    foreach (MCvAvgComp n in nosesDetected[0])
                    {
                        rt.X = (int)(n.rect.X * sf) + f.rect.X + 5;
                        rt.Y = (int)((n.rect.Y + 28) * sf) + f.rect.Y;
                        rt.Width = (int)(30 * sf);
                        rt.Height = (int)(30 * sf);
                        currentFrame.Draw(rt, new Bgr(Color.Red), 2);
                        rt.X = n.rect.X; rt.Y = n.rect.Y + 30;
                        rt.Width = 30;
                        rt.Height = 30;
                        graytam = grayf.Copy(rt).Convert<Gray, byte>();
                        this.ibn.Image = graytam;
                        resultnose = graytam;
                        break;
                    }

                    #endregion

                    //////////////////////////////////////////////////
                    if (matrix1s != null)
                    {
                        string namegb = "", nameAvg = "";
                        double max = 0, avg = 0;
                        Matrix1.Compare(matrix1s, labels, matrixtam, out max, out namegb, out avg, out nameAvg);
                        // name1=recognizerall(f);
                        name1 = namegb;
                        lblTen.Text = namegb;
                        lblMax.Text = string.Format("{0:00.0000}", max);
                        lblAvg.Text = string.Format("{0:00.0000}", avg);
                        lblTenAvg.Text = nameAvg;

                        //Draw the label for each face detected and recognized
                        currentFrame.Draw(namegb, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));


                    }
                    //  NamePersons[t - 1] = name;
                    //  NamePersons.Add("");


                    //Set the number of faces detected on the scene
                    //                label3.Text = facesDetected[0].Length.ToString();

                    // break;
                    SoNguoi++;

                }
                t = 0;
                lblSoNguoi.Text = SoNguoi.ToString();
                //Names concatenation of persons recognized
                //for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                //{
                //    names = names + NamePersons[nnn] + " ";
                //}
                //Show the faces procesed and recognized

                imageBoxframgrabber.Image = currentFrame;

                if (comboBoxEdit1.Text != null && name1 != null)
                    diemdanh();

                //  names = "";
                //Clear the list(vector) of names
                //  NamePersons.Clear();
            }
            catch (Exception exx)
            { //MessageBox.Show("Loi la \n "+exx.ToString()); 
            }
        }

        #region recognize by opencv (eigen face)...
        private string recognizerall(MCvAvgComp f)
        {

            string[] ten = new string[5];
            ten[0] = "";


            if (trainingImages.ToArray().Length != 0)
            {

                //  /Term Criteria for face recognition with numbers of trained images like max Iteration,eps > =>chinh xac
                MCvTermCriteria termCrit = new MCvTermCriteria(tong, 0.6);
                MCvTermCriteria termCritn = new MCvTermCriteria(tong, 0.7);
                MCvTermCriteria termCritm = new MCvTermCriteria(tong, 0.7);
                MCvTermCriteria termCriteL = new MCvTermCriteria(tong, 0.7);
                MCvTermCriteria termCriteR = new MCvTermCriteria(tong, 0.7);
                //Eigen face recognizer 

                EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                   trainingImages.ToArray(),
                   labels.ToArray(),
                   2000,
                   ref termCrit);

                ten[0] = recognizer.Recognize(resultface);
                /*
                
                 ///////////////////////////////////////////////////kiem tra nose/
                 if (resultnose != null)
                 {
                     EigenObjectRecognizer recognizernose = new EigenObjectRecognizer(
                        trainingImagenose.ToArray(),
                        labels.ToArray(),
                        1000,
                        ref termCritn);

                     ten[1] = recognizernose.Recognize(resultnose);
                     currentFrame.Draw("nose: "+ten[1], ref font, new Point(f.rect.X - 2, f.rect.Y - 15), new Bgr(Color.DarkBlue));
                
               
                 }
                 //////////////////////////////////////////////////////////
                
                 if (resultmouth != null)
                 {
                        EigenObjectRecognizer recognizermouth = new EigenObjectRecognizer( 
                        trainingImagemouth.ToArray(),
                        labels.ToArray(),
                        1000,
                        ref termCritm);

                     ten[2] = recognizermouth.Recognize(resultmouth);
                     currentFrame.Draw("mouth: "+ten[2], ref font, new Point(f.rect.X - 2, f.rect.Y - 30), new Bgr(Color.LightGreen));
                 }
 
                 if (resulteyeL != null)
                 {
                     EigenObjectRecognizer recognizereyeL = new EigenObjectRecognizer(
                     trainingImageneyeL.ToArray(),
                     labels.ToArray(),
                     1000,
                     ref termCriteL);

                     ten[3] = recognizereyeL.Recognize(resulteyeL);
                     currentFrame.Draw("eyes: "+ten[3], ref font, new Point(f.rect.X - 45, f.rect.Y - 45), new Bgr(Color.LightGreen));
                 }
                 if (resulteyeR != null)
                 {
                     EigenObjectRecognizer recognizereyeR = new EigenObjectRecognizer(
                     trainingImageneyeR.ToArray(),
                     labels.ToArray(),
                     600,
                     ref termCriteR);

                    ten[4] = recognizereyeR.Recognize(resulteyeR);
                    currentFrame.Draw(ten[4], ref font, new Point(f.rect.X +65, f.rect.Y - 45), new Bgr(Color.LightGreen));
                 }
               

               
             }
            
            
             int tam = 0;
             string name="";
             for (int i = 1; i < 5; i++)
             {
                 if (ten[0] == ten[i]) tam++;
                 if (tam > 2&&ten[0]!=null) { name = ten[0]; break; } else name = "";
             }
                 */
            }
            return ten[0];
        }
        #endregion

        #region draw eye
        private MCvAvgComp DrawEyes(MCvAvgComp f)
        {


            // Our Region of interest where find eyes will start with a sample estimation using face metric
            Int32 StartSearchEyes = f.rect.Top + (f.rect.Height * 3 / 11);
            Point startingPointSearchEyes = new Point(f.rect.X, StartSearchEyes);
            Point endingPointSearchEyes = new Point((f.rect.X + f.rect.Width), StartSearchEyes);

            Size searchEyesAreaSize = new Size(f.rect.Width, (f.rect.Height * 2 / 9));
            Point lowerEyesPointOptimized = new Point(f.rect.X, StartSearchEyes + searchEyesAreaSize.Height);
            Size eyeAreaSize = new Size(f.rect.Width / 2, (f.rect.Height * 2 / 9));
            Point startingLeftEyePointOptimized = new Point(f.rect.X + f.rect.Width / 2, StartSearchEyes);

            Rectangle possibleROI_eyes = new Rectangle(startingPointSearchEyes, searchEyesAreaSize);
            Rectangle possibleROI_rightEye = new Rectangle(startingPointSearchEyes, eyeAreaSize);
            Rectangle possibleROI_leftEye = new Rectangle(startingLeftEyePointOptimized, eyeAreaSize);



            #region Drawing Utilities
            // Let's draw our search area, first the upper line
            currentFrame.Draw(new LineSegment2D(startingPointSearchEyes, endingPointSearchEyes), new Bgr(Color.White), 3);
            // draw the bottom line
            currentFrame.Draw(new LineSegment2D(lowerEyesPointOptimized, new Point((lowerEyesPointOptimized.X + f.rect.Width), (StartSearchEyes + searchEyesAreaSize.Height))), new Bgr(Color.White), 3);
            // draw the eyes search vertical line
            currentFrame.Draw(new LineSegment2D(startingLeftEyePointOptimized, new Point(startingLeftEyePointOptimized.X, (StartSearchEyes + searchEyesAreaSize.Height))), new Bgr(Color.White), 3);

            //     MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
            //     frame.Draw("Search Eyes Area", ref font, new Point((startingLeftEyePointOptimized.X - 80), (StartSearchEyes + searchEyesAreaSize.Height + 15)), new Bgr(Color.Yellow));
            //     frame.Draw("Right Eye Area", ref font, new Point(startingPointSearchEyes.X, startingPointSearchEyes.Y - 10), new Bgr(Color.Aqua));
            //     frame.Draw("Left Eye Area", ref font, new Point(startingLeftEyePointOptimized.X + searchEyesAreaSize.Height / 2, startingPointSearchEyes.Y - 10), new Bgr(Color.Yellow));
            #endregion
            return f;
        }
        #endregion
        bool ktgrabber;
        private void detect_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    detect.Enabled = false;

            //    grabber = new Capture();
            //    grabber.QueryFrame();                
            //    Application.Idle += new EventHandler(FrameGrabber);
            //    addface.Enabled = true;
            //    cancle.Enabled = true;

            //}
            //catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            try
            {
                if (!ktgrabber)
                {
                    detect.Text = "Stop";
                    ktgrabber = true;
                    grabber = new Capture();
                    grabber.QueryFrame();
                    Application.Idle += new EventHandler(FrameGrabber);
                    addface.Enabled = true;
                    //cancle.Enabled = true;
                }
                else
                {
                    grabber.Dispose();
                    Application.Idle -= new EventHandler(FrameGrabber);
                    detect.Text = "Start";
                    ktgrabber = false;
                    addface.Enabled = false;
                   // cancle.Enabled = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void luuanh()
        {
            //  string t = kketnoi.lay1dong("if( select max(hinh) from sinhvien sv) is null  select 0 else select max(hinh)+10 from sinhvien sv");
            // string stt = kketnoi.lay1dong("select max(stt)+1 from sinhvien");
            string matrananh = "";
            int row, col;
            string dtt1 = "";//==================================
            for (int i = 0; i < 10; i++)
            {

                //int tam = Convert.ToInt16(t) + i;
                // tface[i].Save(directorypath + "/face" + tam + ".bmp");
                row = matrix1stam[i].NoRows;
                col = matrix1stam[i].NoCols;

                for (int k = 0; k < row; k++)
                    for (int l = 0; l < col; l++)
                    {
                        matrananh += matrix1stam[i][k, l];
                        dtt1 += matrix1stam[i][k, l];//==============================
                        matrananh += '.';

                    }
                //   TrainedFace.Save(directorypath + "/face" + demfaceluu + ".bmp");
                matrananh += '@';
            }
            //===========================
            //MessageBox.Show(matrananh.ToString());
            // string dtt1 = matrananh.ToString();

            string fileName = Application.StartupPath + "\\dtt1.txt";
            StreamWriter sw = new StreamWriter(fileName, false);
            sw.WriteLine(dtt1);
            sw.Close();

            kketnoi.connect();
            SqlCommand cm = new SqlCommand("insert into sinhvien values('" + mssvtxt.Text + "','" + textBox1.Text + "','','','','','" + matrananh + "','" + loptxt.Text + "')", kketnoi.con);
            cm.ExecuteNonQuery();
            //SqlCommand cm2 = new SqlCommand("insert into diemdanh values('" + mssvtxt.Text + "','" + monhoc_txt.SelectedValue + "','','','','','','','','','','','')", kketnoi.con);
            //cm2.ExecuteNonQuery();
            dt = kketnoi.laydl("select mamh from mon");
            foreach (DataRow dr in dt.Rows)
            {
                kketnoi.connect();
                SqlCommand cm2 = new SqlCommand("insert into diemdanh values('" + mssvtxt.Text + "','" + dr[0].ToString() + "','','','','','','','','','','','')", kketnoi.con);
                cm2.ExecuteNonQuery();
            }
            kketnoi.connectClose();

        }

        private void addface_Click(object sender, EventArgs e)
        {

            try
            {
                if (textBox1.Text == "" | loptxt.Text == "" | mssvtxt.Text == "" | monhoc_txt.Text == "") MessageBox.Show("Chưa nhập đủ thông tin");
                else
                {
                    gray = grabber.QueryGrayFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
             face,
             1.1,
             5,
             Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
             new Size(60, 60));
                    //Action for each element detected
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        //  resultface = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();

                        break;
                    }

                    if (resultface == null) {  timer3.Start(); return; }
                    //resize face detected image for force to compare the same size with the 
                    //test image with cubic interpolation type method
                    TrainedFace = resultface.Resize(100, 100, INTER.CV_INTER_CUBIC);
                    //them ten va face vao mang


                    //Show face added in gray scale
                    //if (dem == 0)
                    imageBox1.Image = TrainedFace;

                    //Write the number of triained faces in a file text for further load
                    //                   File.WriteAllText(Application.StartupPath + "/Faces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                    //Write the labels of triained faces in a file text for further load

                    //      tten.Add("tface" + dem.ToString());
                    try
                    {
                        //TrainedFace.Save(directory + "face" + matrix1s.Count + ".bmp");
                        grabber.QueryFrame().Resize(640, 480, INTER.CV_INTER_CUBIC).Save(directory + textBox1.Text + matrix1s.Count + ".bmp");

                    }
                    catch (Exception ex)
                    {
                        for (int i = matrix1s.Count; i < dem; i++)
                        {
                            // File.Delete(directory + "face" + (matrix1s.Count + dem) + ".bmp");
                            File.Delete(directory + textBox1.Text + (matrix1s.Count + dem) + ".bmp");
                        }
                    }

                    //tface.Add(TrainedFace);
                    TrainedFace = TrainedFace.Resize(50, 50, INTER.CV_INTER_CUBIC);

                    Bitmap tam = TrainedFace.ToBitmap();
                    Bitmap bmnewsize = new Bitmap(tam, newsizegb);
                    x = PCA.image_2_matrix(bmnewsize);
                    x = Radon1.ApdungRadon(x);// PCA.apDungWaveletGabors(x, 0, 1.56, 1);
                    matrix1stam.Add(x);
                    matrix1s.Add(x);
                    labels.Add(textBox1.Text);

                    //foreach (Control c in this.Controls)
                    //{
                    //    if (c is PictureBox & c.Name == face)
                    //    {
                    //        ((PictureBox)c).Image = test;
                    //    }

                    //}
                    //       dt.Clear();

                    /*
                                        for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                                        {
                                            trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/Faces/face" + i + ".bmp");
                                            k = i;
                                            //  File.AppendAllText(Application.StartupPath + "/Faces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");

                                        }

                                        */

                    if (dem != 9)
                        addface.Text = "Add face " + (dem + 2).ToString();
                    dem++;

                    if (dem == 10)
                    {
                        luuanh();
                        MessageBox.Show(textBox1.Text + "'s Face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dem = 0;
                        // tface = null; tface = new List<Image<Gray, byte>>();
                        matrix1stam = null; matrix1stam = new List<Matrix1>();
                        x = null;
                        imageBox1.Image = null;
                        ibe1.Image = ibe2.Image = ibn.Image = ibm.Image = null;
                        addface.Text = "Add face 1";
                        resultface = resulteyeL = resulteyeR = resultmouth = resultnose = null;
                        refreshdata();

                    }
                    HDfaces++;
                    mauso++;
                    if (HDfaces <= 10)
                    {

                        label9.Text = mauso.ToString();
                        pictureBox1.Image = Image.FromFile(Application.StartupPath.ToString() + "/huongdan/" + HDfaces.ToString() + ".bmp");
                        //MessageBox.Show(HDfaces.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                dem = 0;
                MessageBox.Show(ex.ToString(), "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        private void NhanDang_Load(object sender, EventArgs e)
        {
            label9.Text = mauso.ToString();
            pictureBox1.Image = Image.FromFile(Application.StartupPath.ToString() + "/huongdan/" + HDfaces.ToString() + ".bmp");
            try
            {
                laydsmh();

                laydslop();
                refreshdata();
            }
            catch (Exception ex) { MessageBox.Show("Loi ket noi may chu" + ex.ToString()); }
        }
        public void laydsmh()
        {
            kketnoi.connect();
            dt = kketnoi.laydl("select mamh,tenmh from mon");
            monhoc_txt.DataSource = dt;
            monhoc_txt.ValueMember = "mamh";
            monhoc_txt.DisplayMember = "tenmh";
        }
        public void laydslop()
        {
            kketnoi.connect();
            dt = kketnoi.laydl("select malop,tenlop from lop");
            loptxt.DataSource = comboBox1.DataSource = dt;
            loptxt.ValueMember = comboBox1.ValueMember = "malop";
            loptxt.DisplayMember = comboBox1.DisplayMember = "malop";
            //       MessageBox.Show(loptxt.Text + loptxt.GetColumnValue("malop"));
            kketnoi.connectClose();
        }
public void refreshdata()
        {
            mauso = 1;
            HDfaces = 1;
            label9.Text = mauso.ToString();
            pictureBox1.Image = Image.FromFile(Application.StartupPath.ToString() + "/huongdan/" + HDfaces.ToString() + ".bmp");
            string malop = " and sv.malop=" + comboBox1.Text.Trim();
            if (loptxt.Text == "") malop = "";
            //kketnoi.connect();
            //SqlCommand cm = new SqlCommand("exec dssvdd @malop", kketnoi.con);
            //cm.Parameters.AddWithValue("@malop", malop);
            //cm.ExecuteNonQuery();
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cm;
            //da.Fill(dt);
            //gridControl1.DataSource = dt;
            //kketnoi.connectClose();
            gridControl1.DataSource = kketnoi.laydl("select TenMH,sv.MSSV,TenSV,TenLop,Tuan1,Tuan2,Tuan3,Tuan4,Tuan5,Tuan6,Tuan7,Tuan8,Tuan9,Tuan10,Tong from Mon m,sinhvien sv,lop l,DiemDanh d where sv.MaLop=l.MaLop and d.mssv=sv.mssv and m.MaMH=d.MaMH and d.MaMH='" + monhoc_txt.SelectedValue.ToString() + "'" + malop + "");
            // gridControl1.DataSource = kketnoi.laydl("select sv.MSSV,TenSV,TenLop,Tuan1,Tuan2,Tuan3,Tuan4,Tuan5,Tuan6,Tuan7,Tuan8,Tuan9,Tuan10,Tong from sinhvien sv,lop l,DiemDanh d where sv.MaLop=l.MaLop and d.mssv=sv.mssv" + malop + "");
            // gridControl1.DataSource = kketnoi.laydl("select * from sinhvien");
            kketnoi.connectClose();

        }


    

        private void loptxt_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //get all image name  in imagelist

                directorypath = Application.StartupPath + "/Faces/" + loptxt.Text + "";
                if (!Directory.Exists(directorypath))
                {
                    Directory.CreateDirectory(directorypath);
                }
                directory = new DirectoryInfo(directorypath + "/");
                //  Archives = directory.GetFiles("*.bmp");

                loaddanhsachsv();
                refreshdata();
                // MessageBox.Show("lop edit values change");
                // laydl(loptxt.Text);
                //kketnoi.connect();
                //  gridControl1.DataSource = kketnoi.laydl("select sv.MSSV,TenSV,TenLop,Tuan1,Tuan2,Tuan3,Tuan4,Tuan5,Tuan6,Tuan7,Tuan8,Tuan9,Tuan10,Tong from sinhvien sv,lop l,DiemDanh d where sv.MaLop=l.MaLop and d.mssv=sv.mssv and sv.malop='" + loptxt.Text + "' ");
            }
            catch (Exception ex) { MessageBox.Show("Nothings face in databse" + ex.ToString()); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // name2 = name1;
            //   if (timer2.Interval != 1000) timer2.Start();
            //xoa image trong result
            //  resultface = null;

        }
        private void xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string malop = " where sinhvien.malop='" + loptxt.Text + "' and DiemDanh.MSSV=sinhvien.MSSV";
                if (loptxt.Text == "") malop = " where  DiemDanh.MSSV=sinhvien.MSSV";


                kketnoi.connect();
                SqlCommand cm;
                for (int i = 1; i <= 10; i++)
                {
                    cm = new SqlCommand("update diemdanh set Tuan" + i + "='' where mssv=(select mssv from sinhvien " + malop + ") update diemdanh set tong=0", kketnoi.con);
                    cm.ExecuteNonQuery();
                }

                kketnoi.connectClose();
                refreshdata();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void diemdanh()
        {
            try
            {
                if (comboBoxEdit1.Text != "" && name1 != "")
                {
                    string s = kketnoi.lay1dong("select tuan" + comboBoxEdit1.Text + " from sinhvien s, diemdanh d where s.MSSV=d.MSSV and TenSV= '" + name1 + "'" + " and MaMH='" + monhoc_txt.SelectedValue.ToString() + "'"); //co_sua

                    if (s == "x") return;
                    kketnoi.connect();
                    SqlCommand cm = new SqlCommand("update diemdanh set tuan" + comboBoxEdit1.Text + "='x' where MSSV=(select MSSV from Sinhvien where TenSV= '" + name1 + "') and MaMH='" + monhoc_txt.SelectedValue.ToString() + "'", kketnoi.con); //co_sua
                    cm.ExecuteNonQuery();
                    kketnoi.connectClose();
                    refreshdata();
                    //resultface = null;
                    name1 = "";

                }
            }
            catch (Exception) { }
        }
        //reset
     

        private void timer3_Tick(object sender, EventArgs e)
        {

        }

        public void NhanDang_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Canclesaveimage();
            TurnOffCam();
        }

        public void TurnOffCam()
        {
            if (ktgrabber)
            {
                grabber.Dispose();
                Application.Idle -= new EventHandler(FrameGrabber);
                detect.Text = "Start";
                ktgrabber = false;
                addface.Enabled = false;
                //cancle.Enabled = false;
            }
        }

        private void Canclesaveimage()
        {
            try
            {
                if (dem > 0 & dem < 10)
                {
                    for (int i = 0; i < dem; i++)
                    {
                        labels.RemoveAt(labels.Count - 1);
                        matrix1s.RemoveAt(matrix1s.Count - 1);
                    }
                    dem = 0;
                    addface.Text = "Add face 1";
                    // MessageBox.Show(labels.Count.ToString() + matrix1s.Count.ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void diemdanh_Click(object sender, EventArgs e)
        {
            // Canclesaveimage();
            addface.Text = "Add face 1";
        }

       
        private void xoasv()
        {
            try
            {
                demhinh = Convert.ToInt16(kketnoi.lay1dong("select hinh from sinhvien where stt='" + Convert.ToInt16(stt) + "'"));
                int k = Convert.ToInt16(kketnoi.lay1dong("select max(stt) from sinhvien"));
                if (k == Convert.ToInt16(stt))
                {
                    kketnoi.connect();
                    SqlCommand cm = new SqlCommand("delete from sinhvien where stt=" + stt + "", kketnoi.con);
                    cm.ExecuteNonQuery();
                    kketnoi.connectClose();
                    for (int i = 0; i < 3; i++)
                    {
                        File.Delete(Application.StartupPath + "/Faces/face" + demhinh + ".bmp");
                        demhinh++;
                    }
                    LoadImageAndnames();

                    refreshdata();
                }
            }
            catch (Exception) { }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                xoasv();
            }
        }

        #region xuat file excel
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string malop = "and sv.malop=" + loptxt.Text;
                if (loptxt.Text == "") malop = "";

                DataTable dtx = new DataTable();
                kketnoi.connect();
                dtx = kketnoi.laydl("select sv.MSSV,TenSV,TenLop,Tuan1,Tuan2,Tuan3,Tuan4,Tuan5,Tuan6,Tuan7,Tuan8,Tuan9,Tuan10,Tong from sinhvien sv,lop l,DiemDanh d where sv.MaLop=l.MaLop and d.mssv=sv.mssv " + malop + "");
                kketnoi.connectClose();
                ExportTableToExcel.exportToExcel(dtx, "DanhsachSV.xls");
                MessageBox.Show("OK");
                Process.Start("DanhsachSV.xls");
            }
            catch (Exception) { }
        }
        #endregion

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                ExportTableToExcel.exportToExcel((DataTable)gridControl1.DataSource, "DanhsachSV.xls");
                MessageBox.Show("OK");
                Process.Start("DanhsachSV.xls");
            }
            catch (Exception) { }
        }

        private void monhoc_txt_SelectedValueChanged_1(object sender, EventArgs e)
        {
            refreshdata();
        }

        private void loptxt_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                //get all image name  in imagelist

                directorypath = Application.StartupPath + "/Faces/" + loptxt.Text + "";
                if (!Directory.Exists(directorypath))
                {
                    Directory.CreateDirectory(directorypath);
                }
                directory = new DirectoryInfo(directorypath + "/");
                //  Archives = directory.GetFiles("*.bmp");

                loaddanhsachsv();
                refreshdata();
                // MessageBox.Show("lop edit values change");
                // laydl(loptxt.Text);
                //kketnoi.connect();
                //  gridControl1.DataSource = kketnoi.laydl("select sv.MSSV,TenSV,TenLop,Tuan1,Tuan2,Tuan3,Tuan4,Tuan5,Tuan6,Tuan7,Tuan8,Tuan9,Tuan10,Tong from sinhvien sv,lop l,DiemDanh d where sv.MaLop=l.MaLop and d.mssv=sv.mssv and sv.malop='" + loptxt.Text + "' ");
            }
            catch (Exception ex)
            { //MessageBox.Show("Nothings face in databse" + ex.ToString()); 
            }
        }

        private void monhoc_txt_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshdata();
        }

        private void xoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                ExportTableToExcel.exportToExcel((DataTable)gridControl1.DataSource, "DanhsachSV.xls");
                MessageBox.Show("OK");
                Process.Start("DanhsachSV.xls");
            }
            catch (Exception) { }
        }

        private void comboBoxEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void ribbonControl_Click(object sender, EventArgs e)
        {

        }
        ///////////reset
   

        

      

        List<Matrix1> matrix1s = new List<Matrix1>();
        List<Matrix1> matrix1stam = new List<Matrix1>();
        Matrix1 x;
        Matrix1 y, matrixtam;
        Size newsizegb = new Size(30, 30);
        string si;

        public void gabor(Bitmap bmg)
        {

            //Bitmap bmg = new Bitmap(filenamepath);
            Bitmap tam = new Bitmap(bmg, newsizegb);
            x = PCA.image_2_matrix(tam);
            x = Radon1.ApdungRadon(x);//PCA.apDungWaveletGabors(x, 0, 1.56, 1);

        }

        private void mssvtxt_Validated(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt16(kketnoi.lay1dong("select count(mssv) from sinhvien where mssv= '" + mssvtxt.Text + "'")) == 1)
                {
                    MessageBox.Show("MSSV không được trùng");
                    return;
                }
            }
            catch (Exception ei) { MessageBox.Show(ei.ToString()); }

        }

      

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void xóaSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
