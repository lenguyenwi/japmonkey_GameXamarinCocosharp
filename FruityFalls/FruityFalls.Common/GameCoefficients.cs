using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruityFalls
{
    public static class GameCoefficients
    {
        // The height of the bins - the smaller this is, the further down the fruit
        // has to go before its color is checked
        public const float BinHeight = 5;


        // The strength of the gravity. Making this a 
        // smaller (bigger negative) number will make the
        // fruit fall faster. A larger (smaller negative) number
        // will make the fruit more floaty.
        public const float FruitGravity = -60;


        // Controls fruit collision bouncyness. A value of 1
        // means no momentum is lost.  A value of 0 means all
        // momentum is lost. Values greater than 1 create a spring-like effect
        public const float FruitCollisionElasticity = .5f;

        public const float FruitRadius = 8*3;// gap 3 lan luc dau

        public const float StartingFruitPerSecond = .5f;

        // This variable controls how many seconds must pass
        // before another fruit-per-second is added. For example, 
        // if the game initially spawns one fruit per 5 seconds, then 
        // the spawn rate is .2 fruit per second. If this value is 60, that
        // means that after 1 minute, the spawn rate will be 1.2 fruit per second.
        // Initial playtesting suggest that this value should be fairly large like 3+ 
        // minutes (180 seconds) or else the game gets hard 
        public const float TimeForExtraFruitPerSecond = 6 * 60;

        // This controls whether debug information is displayed on screen.
        public const bool ShowDebugInfo = true;

		public const bool ShowCollisionAreas = false;

        // The amount of time which must pass between bonus point awarding. 
        // Without this, the user can earn bonus points every frame
        public const float MinPointAwardingFrequency = .6f;

        //fruit bin collisionHeight ( do rong cua Y trong vu va cham voi Fruit)
        public const float collisionHeight = 1;
        //gateRadius chinh la ban kinh cua Cua de khi vuot qua
        public const float gateRadius = 50;//bang 90 la hop ly
        // luc hap dan chieu X
        public const float gravityXacer = -30;
        //ban kinh cua Paddle can phai chu y khi thay doi hinh anh chinh (Monkey)
        public const float PaddleRadius = 10;
    }
}
