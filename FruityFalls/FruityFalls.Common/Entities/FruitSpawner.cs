using CocosSharp;
using System;
using System.Collections.Generic;

namespace FruityFalls.Entities
{
	public class FruitSpawner
	{
		float timeSinceLastSpawn;
		public float TimeInbetweenSpawns
		{
			get;
			set;
		}

        public string DebugInfo
        {
            get
            {
                string toReturn =
					"Fruit per second: " + (1 / TimeInbetweenSpawns);

                return toReturn;
            }
        }

        public CCLayer Layer
        {
            get;
            set;
        }

		public Action<Fruit> FruitSpawned;

        public bool IsSpawning
        {
            get;
            set;
        }

		public FruitSpawner ()
		{
            IsSpawning = true;
            TimeInbetweenSpawns = 1 / GameCoefficients.StartingFruitPerSecond;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
		}

		public void Activity(float frameTime)
		{
            if (IsSpawning)
            {
                SpawningActivity(frameTime);

                SpawnReductionTimeActivity(frameTime);
            }
		}

        private void SpawningActivity(float frameTime)
        {
            timeSinceLastSpawn += frameTime;

            if (timeSinceLastSpawn > TimeInbetweenSpawns)
            {
                timeSinceLastSpawn -= TimeInbetweenSpawns;

                Spawn();
            }
        }

        private void SpawnReductionTimeActivity(float frameTime)
        {
            // This logic should increase how frequently fruit appear, but it should do so
            // such that the # of fruit/minute increases at a decreasing rate, otherwise the
            // game becomes impossibly difficult very quickly.
            var currentFruitPerSecond = 1 / TimeInbetweenSpawns;

            var amountToAdd = frameTime / GameCoefficients.TimeForExtraFruitPerSecond;

            var newFruitPerSecond = currentFruitPerSecond + amountToAdd;

            TimeInbetweenSpawns = 1 / newFruitPerSecond;

        }

        // made public for debugging, may make it private later:
        private void Spawn()
		{
            float gateMiddlePoint= Layer.ContentSize.Width / 2;
            gateMiddlePoint = CCRandom.GetRandomFloat(0+ GameCoefficients.FruitRadius+ GameCoefficients.gateRadius * 2, Layer.ContentSize.Width- GameCoefficients.FruitRadius - GameCoefficients.gateRadius*2);
            //tao red dau tien ben trai
            createNewFruitRed(0 + GameCoefficients.FruitRadius);
             for (float i = GameCoefficients.FruitRadius; i < Layer.ContentSize.Width - GameCoefficients.FruitRadius; i += GameCoefficients.FruitRadius*2)       
            //for (float i = 0; i < Layer.ContentSize.Width + GameCoefficients.FruitRadius; i += GameCoefficients.FruitRadius * 2)
            {

                ////tao canh cua de khi vuot qua dau tien la sac dinh tam bang gateMiddlePoint sau do la tao fruit(vatcan) tru di ban kinh cua gate
                //gateMiddlePoint = Layer.ContentSize.Width / 2; //Layer.ContentSize.Width / 2;


                //createNewFruitRed(gateMiddlePoint);
                if (i < gateMiddlePoint - GameCoefficients.gateRadius * 2 || i > gateMiddlePoint + GameCoefficients.gateRadius * 2)
                {
                    createNewFruitRed(i);
                }
                else
                {
                    creaNewFruitYellow(i);
                }
                //if ((0 < i && i < gateMiddlePoint - GameCoefficients.gateRadius * 2) || (i > gateMiddlePoint + GameCoefficients.gateRadius * 2 && i < Layer.ContentSize.Width))
                //{
                //    createNewFruitRed(i);
                //}
                //else if (i != 0 + GameCoefficients.FruitRadius)//khong tao yellow dau tien ben trai vi nhu the se de len mau do da tao len dau tien
                //{
                //    creaNewFruitYellow(i);
                //}

            }
            //tao red cuoi cung ben phai
            createNewFruitRed(Layer.ContentSize.Width - GameCoefficients.FruitRadius);

           
        }

        private void creaNewFruitYellow(float i)
        {
            // create yellow fruit
            var fruit = new Fruit();
            if (Layer == null)
            {
                throw new InvalidOperationException("Need to set Layer before spawning");
            }
            //fruit.PositionX = CCRandom.GetRandomFloat(0 + fruit.Radius, Layer.ContentSize.Width - fruit.Radius);
            fruit.PositionX = i;
            //fruit.PositionY = Layer.ContentSize.Height + fruit.Radius;
            fruit.PositionY = 0 + GameCoefficients.FruitRadius;


            fruit.FruitColor = FruitColor.Yellow;
            if (FruitSpawned != null)
            {
                FruitSpawned(fruit);

            }
        }

        public void createNewFruitRed(float fruitPositionX)
        {
            var fruit = new Fruit();

            if (Layer == null)
            {
                throw new InvalidOperationException("Need to set Layer before spawning");
            }

            //fruit.PositionX = CCRandom.GetRandomFloat(0 + fruit.Radius, Layer.ContentSize.Width - fruit.Radius);
            fruit.PositionX = fruitPositionX;
            //fruit.PositionY = Layer.ContentSize.Height + fruit.Radius;
            fruit.PositionY = 0 + GameCoefficients.FruitRadius;


            fruit.FruitColor = FruitColor.Red;
        
            


            if (FruitSpawned != null)
            {
                FruitSpawned(fruit);
              
            }
        }



	}
}

