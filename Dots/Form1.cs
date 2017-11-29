using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Help.Data;
using Help;
using UnityEngine;

namespace Dots
{
    public partial class Dots : Form
    {
        RustInterceptor inty;
        public static bool firstEnt = false;
        private Timer timer;
        private int countdown = 100;
        private static float myX;
        private static float myY;
        private static float myZ;
        private static float myDirx = 0;
        private static float myDiry = 0;
        private static float myDirz;
        public static Pen linePen = new Pen(System.Drawing.Color.Black, 1);

        public float zoom = 1.0F;
        public static List<Entity> entities = new List<Entity>();
        public static List<Entity> sleepers = new List<Entity>();
        public static List<Entity> resources = new List<Entity>();
        public static List<Entity> stashes = new List<Entity>();
        public static List<Entity> guns = new List<Entity>();
        public static List<Entity> cloth = new List<Entity>();
        public static List<Entity> items = new List<Entity>();

        public Dots()
        {
            InitializeComponent();
            RadarBox.Paint += new PaintEventHandler(this.refreshRadar);

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(ticker);
            timer.Start();

        }

        void ticker(object sender, EventArgs e)
        {
            countdown -= 100;
            if (countdown < 0)
            {
                //Console.WriteLine("Timer done");
                countdown = 100;
                update();
            }
        }

        private void update()
        {
            //Console.WriteLine("updating");
            RadarBox.Invalidate();
        }

        public void refreshRadar(object sender, PaintEventArgs e)
        {
            //Console.WriteLine("attempting to refresh");
            Image playerMark = global::Dots.Properties.Resources.playermark;
            Image sulfurMark = global::Dots.Properties.Resources.YellowDot;
            Image metalMark = global::Dots.Properties.Resources.OrangeDot;
            Image stoneMark = global::Dots.Properties.Resources.GreenDot;
            Image stashMark = global::Dots.Properties.Resources.BrownDot;
            Image itemMark = global::Dots.Properties.Resources.BlueDot;
            Image sleeperMark = global::Dots.Properties.Resources.PurpleDot;

            System.Drawing.Font Arial = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            Brush brush = Brushes.Blue;
            int midX = RadarBox.Width / 2 - 5;
            int midY = RadarBox.Height / 2 - 5;

            if (entities != null)
            {
                try
                {
                    foreach (Entity player in entities.ToList())
                    {

                        float theirX = player.Data.baseEntity.pos.x;
                        float theirY = player.Data.baseEntity.pos.z;
                        float diffX = (theirX - myX) / zoom;
                        float diffY = (theirY - myY) * -1 / zoom;
                        float drawX = (midX + diffX);
                        float drawY = (midY + diffY);
                        float myRads = myDiry * (float)Math.PI / 180;
                        //float cosTheta = (float)Math.Cos(myAngle);
                        //float sinTheta = (float)Math.Sin(myAngle);
                        float theirAngle = (float)Math.Atan2(drawX - midX, drawY - midY);
                        float theirRelativeAngle = myRads - theirAngle;
                        float theirDistance = (float)Math.Sqrt(((drawX - midX) * (drawX - midX)) + ((drawY - midY) * (drawY - midY)));
                        //float x = (float)(Math.Cos(rads) * (drawX - midX) - Math.Sin(rads) * (drawY - midY) + midX);
                        //float y = (float)(Math.Sin(rads) * (drawX - midX) - Math.Cos(rads) * (drawY - midY) + midX);
                        float finalX = midX + (theirDistance * (float)Math.Sin(theirRelativeAngle));
                        float finalY = midY + (theirDistance * (float)Math.Cos(theirRelativeAngle));
                        //float x = midX + distance * (float)Math.Cos(rads);
                        //float y = midY + distance * (float)Math.Sin(rads);
                        //Console.WriteLine("Attempting to draw entitity: " + x + " and " + y);
                        e.Graphics.DrawImage(playerMark, drawX, drawY, 10, 10);
                        e.Graphics.DrawString(player.Data.basePlayer.name, Arial, brush, drawX + 5, drawY + 5);
                        //e.Graphics.DrawString(player.Data.basePlayer.health.ToString(), Arial, brush, drawX + 5, drawY + 13);

                    }
                }
                catch (Exception ex)
                {
                    //Do nothing
                }
                entities.Clear();
                ZoomBox.Text = zoom.ToString();
            }

            // Resource List
            if ((resources != null) && (ResourcesBox.Checked == true))
            {
                try
                {
                    foreach (Entity res in resources.ToList())
                    {
                        float theirX = res.Data.baseEntity.pos.x;
                        float theirY = res.Data.baseEntity.pos.z;
                        float diffX = (theirX - myX) / zoom;
                        float diffY = (theirY - myY) * -1 / zoom;
                        float drawX = midX + diffX;
                        float drawY = midY + diffY;
                        //e.Graphics.DrawString(res.Data.baseNetworkable.prefabID.ToString(), Arial, brush, drawX + 5, drawY + 5
                        if (res.IsPlayer)
                        {
                            e.Graphics.DrawString(res.Data.basePlayer.name.ToString(), Arial, brush, drawX, drawY+5);
                        }
                        // Sulfur
                        if (res.Data.baseNetworkable.prefabID.ToString().Equals("1351790091") || res.Data.baseNetworkable.prefabID.ToString().Equals("1502989249"))
                        {
                            if (SulfurBox.Checked == true)
                            {
                                e.Graphics.DrawImage(sulfurMark, drawX, drawY, 10, 10);
                            }
                        }

                        // Metal
                        if (res.Data.baseNetworkable.prefabID.ToString().Equals("736323343") || res.Data.baseNetworkable.prefabID.ToString().Equals("2680863385"))
                        {
                            if (MetalBox.Checked == true)
                            {
                                e.Graphics.DrawImage(metalMark, drawX, drawY, 10, 10);
                            }
                        }

                        // Stone
                        if (res.Data.baseNetworkable.prefabID.ToString().Equals("916501233") || res.Data.baseNetworkable.prefabID.ToString().Equals("2861041275"))
                        {
                            if (StoneBox.Checked == true)
                            {
                                e.Graphics.DrawImage(stoneMark, drawX, drawY, 10, 10);
                            }
                        }

                        //e.Graphics.DrawString(res.Data.baseNetworkable.prefabID.ToString(), Arial, brush, drawX + 5, drawY + 5);

                    }
                }

                catch (Exception idk)
                {
                    //do nothing
                }
            }

            if ((stashes != null) && (StashesBox.Checked == true))
            {
                foreach (Entity stash in stashes.ToList())
                {
                    float theirX = stash.Data.baseEntity.pos.x;
                    float theirY = stash.Data.baseEntity.pos.z;
                    float diffX = (theirX - myX) / zoom;
                    float diffY = (theirY - myY) * -1 / zoom;
                    float drawX = midX + diffX;
                    float drawY = midY + diffY;

                    e.Graphics.DrawImage(stashMark, drawX, drawY, 10, 10);
                    //e.Graphics.DrawString(stash.Data.baseEntity.pos.y.ToString(), Arial, brush, drawX + 5, drawY + 5);
                    //e.Graphics.DrawString(myZ.ToString(), Arial, brush, drawX + 10, drawY + 10);
                }
            }

            if ((cloth != null) && (PlantsBox.Checked == true))
            {
                foreach (Entity plant in cloth.ToList())
                {
                    float theirX = plant.Data.baseEntity.pos.x;
                    float theirY = plant.Data.baseEntity.pos.z;
                    float diffX = (theirX - myX) / zoom;
                    float diffY = (theirY - myY) * -1 / zoom;
                    float drawX = midX + diffX;
                    float drawY = midY + diffY;

                    e.Graphics.DrawImage(stashMark, drawX, drawY, 10, 10);
                    //e.Graphics.DrawString(stash.Data.baseEntity.pos.y.ToString(), Arial, brush, drawX + 5, drawY + 5);
                    //e.Graphics.DrawString(myZ.ToString(), Arial, brush, drawX + 10, drawY + 10);
                }
            }

            if ((sleepers != null) && (BodiesBox.Checked == true))
            {
                foreach (Entity body in sleepers.ToList())
                {
                    float theirX = body.Data.baseEntity.pos.x;
                    float theirY = body.Data.baseEntity.pos.z;
                    float diffX = (theirX - myX) / zoom;
                    float diffY = (theirY - myY) * -1 / zoom;
                    float drawX = midX + diffX;
                    float drawY = midY + diffY;

                    e.Graphics.DrawImage(playerMark, drawX, drawY, 10, 10);
                    //e.Graphics.DrawString(body.Data.basePlayer.name, Arial, brush, drawX + 5, drawY + 5);

                }
            }

            if ((items != null) && (itemsBox.Checked == true))
            {
                foreach (Entity body in items.ToList())
                {
                    float theirX = body.Data.baseEntity.pos.x;
                    float theirY = body.Data.baseEntity.pos.z;
                    float diffX = (theirX - myX) / zoom;
                    float diffY = (theirY - myY) * -1 / zoom;
                    float drawX = midX + diffX;
                    float drawY = midY + diffY;

                    e.Graphics.DrawImage(playerMark, drawX, drawY, 10, 10);
                    e.Graphics.DrawString(body.Data.baseNetworkable.prefabID.ToString(), Arial, brush, drawX + 5, drawY + 5);

                }
            }
            // Your Dot
            float lineX = (RadarBox.Width / 2 - 5) + (50 * ((float)Math.Sin(myDiry * (float)Math.PI / 180)));
            float lineY = (RadarBox.Height / 2 - 5) + (50 * (-1 * (float)Math.Cos(myDiry * (float)Math.PI / 180)));
            e.Graphics.DrawImage(playerMark, RadarBox.Width / 2 - 5, RadarBox.Height / 2 - 5, 10, 10);
            e.Graphics.DrawLine(linePen, RadarBox.Width / 2, RadarBox.Height / 2, lineX, lineY);
            //e.Graphics.DrawString(myDirx.ToString() + " " + myDiry + " " + myDirz, Arial, brush, midX + 5, midY + 5);
        }

        private static void PacketReceived(Packet packet)
        {
            Entity enty;
            //Console.WriteLine("bruh, we gettin packets");
            //Console.WriteLine(packet.GetName());
            /*
            if (packet.rustID == Packet.Rust.Entities)
            {
                Console.WriteLine(packet);
            }
            */

            if (packet.rustID == Packet.Rust.Entities)
            {
                ProtoBuf.Entity entityInfo;
                uint num = Entity.ParseEntity(packet, out entityInfo);
                enty = Entity.CreateOrUpdate(num, entityInfo);
                if (enty != null) OnEntity(enty);
                return;
            }

            if (packet.rustID == Packet.Rust.EntityPosition)
            {
                List<Entity.EntityUpdate> updates = Entity.ParsePositions(packet);
                //Console.WriteLine(updates);
                List<Entity> entities = null;
                if (updates.Count == 1)
                {
                    enty = Entity.UpdatePosistion(updates[0]);
                    if (enty != null) (entities = new List<Entity>()).Add(enty);
                }
                else if (updates.Count > 1)
                {
                    entities = Entity.UpdatePositions(updates);
                }
                if (entities != null) entities.ForEach(item => OnEntity(item));
                return;
            }

            else if (packet.rustID == Packet.Rust.EntityDestroy)
            {
                EntityDestroy destroyInfo = new EntityDestroy(packet);
                Entity.CreateOrUpdate(destroyInfo);
                EntityDestroyed(destroyInfo);
                return;
            }
        }

        private static void OnEntity(Entity enty)
        {
            if (enty.IsPlayer)
            {
                if (enty.IsLocalPlayer)
                {
                    //Console.WriteLine("Found you " + enty.Data.basePlayer.name + " at" + enty.Data.baseEntity.pos);
                    myX = enty.Data.baseEntity.pos.x;
                    myY = enty.Data.baseEntity.pos.z;
                    myZ = enty.Data.baseEntity.pos.y;

                    myDirx = enty.Data.baseEntity.rot.x;
                    myDiry = enty.Data.baseEntity.rot.y;
                    myDirz = enty.Data.baseEntity.rot.z;

                }
                else
                {
                    //Console.WriteLine("adding entity");
                    if (enty.Data.basePlayer.modelState.sleeping == true)
                    {
                        Console.WriteLine("sleeper found");
                        sleepers.Add(enty);
                        resources.Add(enty);
                    }
                    else
                    {
                        entities.Add(enty);
                    }

                    //Console.WriteLine("Found " + enty.Data.basePlayer.name + " at" + enty.Data.baseEntity.pos);
                }
            }
            else if (enty.IsResource)
            {
                // Sulfur
                if (enty.Data.baseNetworkable.prefabID.ToString().Equals("1351790091") || enty.Data.baseNetworkable.prefabID.ToString().Equals("1502989249"))
                {
                    
                }
                //Console.WriteLine("Resrouces found");
                /*
                if (enty.Data.baseNetworkable.prefabID.ToString().Equals("1351790091") || enty.Data.baseNetworkable.prefabID.ToString().Equals("1502989249")){
                    resources.Add(enty);*/


                resources.Add(enty);

            }
            
            else if (enty.IsWorldItem)
            {
                items.Add(enty);
                //Console.WriteLine(enty.Data.worldItem.item.name);
            }
            else if (enty.IsSleeper)
            {
                sleepers.Add(enty);
                //Console.WriteLine("Sleeper found");
            }
            else if (enty.IsEntity)
            {
                if (enty.Data.baseNetworkable.prefabID == 3577840533)
                {
                    Console.WriteLine("Box found");
                    stashes.Add(enty);
                }
                else if (enty.Data.baseNetworkable.prefabID == 3439001196)
                {
                    stashes.Add(enty);
                }
                else if (enty.Data.baseNetworkable.prefabID == 3278044659)
                {
                    cloth.Add(enty);
                }
                else if (enty.Data.baseNetworkable.prefabID == 1327435419)
                {
                    cloth.Add(enty);
                }
                //else
                //resources.Add(enty);

                //Console.WriteLine("ENTITY");
                items.Add(enty);
            }
            else if (enty.IsNPC)
            {
                //resources.Add(enty);
                //Console.WriteLine("NPC found");
            }
            /*
            else if (enty.IsPlant)
            {
                Console.WriteLine("Plant found");
                if (enty.Data.baseNetworkable.prefabID.ToString().Equals("3278044659"))
                {
                    Console.WriteLine("Hemp Found");
                    //resources.Add(enty);
                }
                resources.Add(enty);

            }*/
        }

        private static void EntityDestroyed(EntityDestroy enty)
        {
            //Console.WriteLine("Entity destroyed");
        }

        public static float[,] getViewMatrix(Vector3 eye, float pitch, float yaw)
        {
            //float[,] viewMatrix = new float[4,4];
            float cosPitch = (float)Math.Cos(pitch);
            float sinPitch = (float)Math.Sin(pitch);
            float cosYaw = (float)Math.Cos(yaw);
            float sinYaw = (float)Math.Sin(yaw);

            Vector3 xAxis = new Vector3(cosYaw, 0, -sinYaw);
            Vector3 yAxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            Vector3 zAxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            float[,] viewMatrix = { 
                { xAxis.x, yAxis.x, zAxis.x, 0}, 
                { xAxis.y, yAxis.y, zAxis.y, 0}, 
                { xAxis.z, yAxis.z, zAxis.z, 0}, 
                { Vector2.Dot(xAxis, eye), Vector2.Dot(yAxis, eye), Vector2.Dot(xAxis, eye), 1}
            };

            return viewMatrix;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zoom >= 1)
            {
                zoom += 1;
            }
            else
            {
                zoom *= 2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zoom <= 1)
            {
                zoom /= 2;
            }
            else
            {
                zoom -= 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            entities.Clear();
            resources.Clear();
            stashes.Clear();
            cloth.Clear();
        }

        private void BlueberryButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "144.217.146.243";
            PortBox.Text = "28015";
        }

        private void SavasButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "162.248.88.203";
            PortBox.Text = "28074";
            /*
            inty = new RustInterceptor("162.248.88.203", 28074);
            inty.ClientPackets = false;
            inty.RememberPackets = false;
            inty.AddPacketsToFilter(Packet.Rust.Entities, Packet.Rust.EntityDestroy, Packet.Rust.EntityPosition);
            inty.RegisterCallback(PacketReceived);
            //inty.commandCallback = OnCommand;
            //inty.packetHandlerCallback = internalOnPacket;
            inty.Start();
            */
        }

        private void TntButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "104.153.105.138";
            PortBox.Text = "28026";
            /*
            inty = new RustInterceptor("104.153.105.138", 28026);
            inty.ClientPackets = false;
            inty.RememberPackets = false;
            inty.AddPacketsToFilter(Packet.Rust.Entities, Packet.Rust.EntityDestroy, Packet.Rust.EntityPosition);
            inty.RegisterCallback(PacketReceived);
            //inty.commandCallback = OnCommand;
            //inty.packetHandlerCallback = internalOnPacket;
            inty.Start();*/
        }

        private void DoOrDieButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "74.91.125.225";
            PortBox.Text = "28026";
        }

        private void UberButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "45.58.124.202";
            PortBox.Text = "28025";
        }

        private void RustafiedMainButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "192.223.26.55";
            PortBox.Text = "26032";

        }

        private void VikingButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "74.91.124.27";
            PortBox.Text = "28015";
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            IPBox.Text = "74.91.122.115";
            PortBox.Text = "28069";
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            inty = new RustInterceptor(IPBox.Text, int.Parse(PortBox.Text));
            inty.ClientPackets = false;
            inty.RememberPackets = false;
            inty.AddPacketsToFilter(Packet.Rust.Entities, Packet.Rust.EntityDestroy, Packet.Rust.EntityPosition);
            inty.RegisterCallback(PacketReceived);
            //inty.commandCallback = OnCommand;
            //inty.packetHandlerCallback = internalOnPacket;
            inty.Start();
        }
    }

    }
