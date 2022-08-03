using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSkiaParticleGardenDemo
{
public class Ball
{
    public double X { get; set; }
    public double Y { get; set; }
    public double VX { get; set; }
    public double VY { get; set; }
    public int Radius { get; set; }
    public int Move { get; set; }
    public SKColor sKColor { get; set; } = SKColors.Blue;
    /// <summary>
    /// 检查球的碰撞
    /// </summary>
    public static void CheckBallHit(Ball b1, Ball b2)
    {
        var dx = b2.X - b1.X;
        var dy = b2.Y - b1.Y;
        var dist = Math.Sqrt(Math.Pow(dx,2) + Math.Pow(dy, 2));
        if (dist < b1.Radius + b2.Radius)
        {
            var angle = Math.Atan2(dy, dx);
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);

            // 以b1为参照物，设定b1的中心点为旋转基点
            double x1 = 0;
            double y1 = 0;
            var x2 = dx * cos + dy * sin;
            var y2 = dy * cos - dx * sin;

            // 旋转b1和b2的速度
            var vx1 = b1.VX * cos + b1.VY * sin;
            var vy1 = b1.VY * cos - b1.VX * sin;
            var vx2 = b2.VX * cos + b2.VY * sin;
            var vy2 = b2.VY * cos - b2.VX * sin;

            // 求出b1和b2碰撞之后的速度
            var vx1Final = ((b1.Move - b2.Move) * vx1 + 2 * b2.Move * vx2) / (b1.Move + b2.Move);
            var vx2Final = ((b2.Move - b1.Move) * vx2 + 2 * b1.Move * vx1) / (b1.Move + b2.Move);

            // 处理两个小球碰撞之后，将它们进行归位
            var lep = (b1.Radius + b2.Radius) - Math.Abs(x2 - x1);

            x1 = x1 + (vx1Final < 0 ? -lep / 2 : lep / 2);
            x2 = x2 + (vx2Final < 0 ? -lep / 2 : lep / 2);

            b2.X = b1.X + (x2 * cos - y2 * sin);
            b2.Y = b1.Y + (y2 * cos + x2 * sin);
            b1.X = b1.X + (x1 * cos - y1 * sin);
            b1.Y = b1.Y + (y1 * cos + x1 * sin);

            b1.VX = vx1Final * cos - vy1 * sin;
            b1.VY = vy1 * cos + vx1Final * sin;
            b2.VX = vx2Final * cos - vy2 * sin;
            b2.VY = vy2 * cos + vx2Final * sin;
        }
    }
}
}
