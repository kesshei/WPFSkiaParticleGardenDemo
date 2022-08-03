using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSkiaParticleGardenDemo
{
    /// <summary>
    /// 粒子花园
    /// </summary>
    public class ParticleGarden
    {
        public SKPoint centerPoint;
        public double Spring = 0.0001;
        public int ParticelCount = 100;
        public List<Ball> Particles = new List<Ball>();
        public SKCanvas canvas;
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render(SKCanvas canvas, SKTypeface Font, int Width, int Height)
        {
            this.canvas = canvas;
            canvas.Clear(SKColors.Black);
            centerPoint = new SKPoint(Width / 2, Height / 2);
            if (Particles.Any() == false)
            {
                for (int i = 0; i < ParticelCount; i++)
                {
                    Random random = new Random((int)DateTime.Now.Ticks);
                    var Length = random.Next(3, 10);
                    Particles.Add(new Ball()
                    {
                        X = random.Next(0, Width),
                        Y = random.Next(0, Height),
                        sKColor = SKColors.White,
                        VX = random.NextInt64(-2, 2),
                        VY = random.NextInt64(-2, 2),
                        Radius = Length,
                        Move = Length
                    });
                }
            }

            //画线
            for (int i = 0; i < Particles.Count; i++)
            {
                Move(Particles[i], i, Width, Height);
            }
            //画球
            foreach (var item in Particles)
            {
                DrawCircle(canvas, item);
            }

            using var paint = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 24
            };
            string by = $"by 蓝创精英团队";
            canvas.DrawText(by, 600, 400, paint);
        }
        public void Move(Ball p, int i, int width, int height)
        {
            p.X += p.VX;
            p.Y += p.VY;

            for (var j = i + 1; j < Particles.Count; j++)
            {
                var target = Particles[j];
                CheckSpring(p, target, width, height);
                Ball.CheckBallHit(p, target);
            }

            if (p.X - p.Radius > width)
            {
                p.X = -p.Radius;
            }
            else if (p.X + p.Radius < 0)
            {
                p.X = width + p.Radius;
            }
            if (p.Y - p.Radius > height)
            {
                p.Y = -p.Radius;
            }
            else if (p.Y + p.Radius < 0)
            {
                p.Y = height + p.Radius;
            }
        }
        public void CheckSpring(Ball current, Ball target, int width, int height)
        {
            var dx = target.X - current.X;
            var dy = target.Y - current.Y;
            var dist = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            var minDist = width > height ? width / 10 : height / 5;
            if (dist < minDist)
            {
                DrawLine(current, target, dist, minDist);
                var ax = dx * Spring;
                var ay = dy * Spring;
                current.VX += ax / current.Move;
                current.VY += ay / current.Move;
                target.VX -= ax / target.Move;
                target.VY -= ay / target.Move;
            }
        }

        public void DrawLine(Ball current, Ball target, double dist, int minDist)
        {
            var StrokeWidth = (float)(2 * Math.Max(0, (1 - dist / minDist)));
            var Alpha = Math.Max(0, (1 - dist / minDist)) * byte.MaxValue;
            var Color = current.sKColor.WithAlpha((byte)Alpha);

            //划线
            using var LinePaint = new SKPaint
            {
                Color = Color,
                Style = SKPaintStyle.Fill,
                StrokeWidth = StrokeWidth,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
            var path = new SKPath();
            path.MoveTo((float)current.X, (float)current.Y);
            path.LineTo((float)target.X, (float)target.Y);
            path.Close();
            canvas.DrawPath(path, LinePaint);
        }
        /// <summary>
        /// 画一个圆
        /// </summary>
        public void DrawCircle(SKCanvas canvas, Ball ball)
        {
            using var paint = new SKPaint
            {
                Color = ball.sKColor,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                StrokeWidth = 2
            };
            canvas.DrawCircle((float)ball.X, (float)ball.Y, ball.Radius, paint);
        }
    }
}
