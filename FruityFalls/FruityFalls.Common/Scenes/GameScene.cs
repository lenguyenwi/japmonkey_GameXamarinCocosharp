using System;
using CocosSharp;
using FruityFalls.Entities;
using System.Collections.Generic;
using FruityFalls.Geometry;

namespace FruityFalls.Scenes
{
	public class GameScene : CCScene
	{
        CCLayer backgroundLayer;
		CCLayer gameplayLayer;
        CCLayer foregroundLayer;
        CCLayer hudLayer;

        int score = 0;
        ScoreText scoreText;

		Paddle paddle;
		List<Fruit> fruitList;

        

		FruitSpawner spawner;

    

		List<FruitBin> fruitBins;
        private bool hasGameEnded;

        CCLabel debugLabel;


        //new variable
        float frameTimeInSeconds;
        float paddleXVerlocity;
        float paddleYVerlocity;
        float gravityX = 300;//de test thi 60
        //float gravityY = -300;
        public GameScene(CCGameView gameView) : base(gameView)
        {
            CreateLayers();

            fruitList = new List<Fruit>();

            CreateBackground();

            CreatePaddle();

            CreateBins();

            CreateForeground();

            CreateTouchListener();

            CreateHud();

            CreateSpawner();

            CreateDebugLabel();

            Schedule(Activity);

        }

        private void CreateForeground()
        {
            var foreground = new CCSprite("foreground.png");
            foreground.IsAntialiased = false;
            foreground.AnchorPoint = new CCPoint(0, 0);
            foregroundLayer.AddChild(foreground);

            if(GameCoefficients.ShowCollisionAreas)
            {
                // Make it transparent so collision areas are easier to see:
                foreground.Opacity = 100;
            }
        }

        private void CreateDebugLabel()
        {
            debugLabel = new CCLabel("DebugLabel", "Arial", 20, CCLabelFormat.SystemFont);
            debugLabel.PositionX = hudLayer.ContentSize.Width/2.0f;
            debugLabel.PositionY = 650;
            debugLabel.HorizontalAlignment = CCTextAlignment.Left;
            if (GameCoefficients.ShowDebugInfo)
            {
                hudLayer.AddChild(debugLabel);
            }
        }

        private void CreateBackground()
        {
            var background = new CCSprite("background.png");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = false;
            backgroundLayer.AddChild(background);
        }

        private void CreateLayers()
        {
            backgroundLayer = new CCLayer();
            this.AddLayer(backgroundLayer);

            gameplayLayer = new CCLayer();
            this.AddLayer(gameplayLayer);

            foregroundLayer = new CCLayer();
            this.AddLayer(foregroundLayer);

            hudLayer = new CCLayer();
            this.AddLayer(hudLayer);
        }



        private void CreateHud()
        {
            scoreText = new ScoreText();
            scoreText.PositionX = 10;
            scoreText.PositionY = hudLayer.ContentSize.Height - 30;
            scoreText.Score = 0;
            hudLayer.AddChild(scoreText);
        }


        private void CreateSpawner()
        {
            spawner = new FruitSpawner();
            spawner.FruitSpawned += HandleFruitAdded;
            spawner.Layer = gameplayLayer;
        }

        private void CreatePaddle()
        {
            paddle = new Paddle();
            paddle.PositionX = gameplayLayer.ContentSize.Width / 2.0f;
            paddle.PositionY = gameplayLayer.ContentSize.Height-50;

            paddle.SetDesiredPositionToCurrentPosition();

            gameplayLayer.AddChild(paddle);
        }

        private void HandleFruitAdded(Fruit fruit)
		{
			fruitList.Add (fruit);
			gameplayLayer.AddChild (fruit);
		}

		private void CreateBins()
		{
			CCGeometryNode node;

			var gameView = base.GameView;

			fruitBins = new List<FruitBin>();
			
			// make 2 bins for now:
			var bin = new FruitBin ();
			bin.FruitColor = FruitColor.Red;
			bin.Width = gameView.DesignResolution.Width / 2;
            bin.PositionY = gameView.DesignResolution.Height-GameCoefficients.collisionHeight;
            fruitBins.Add (bin);
			gameplayLayer.AddChild(bin);

			bin = new FruitBin ();
			bin.FruitColor = FruitColor.Yellow;
			// todo: use the screen width to assign this:
			bin.PositionX = gameView.DesignResolution.Width / 2;
            bin.PositionY = gameView.DesignResolution.Height - GameCoefficients.collisionHeight;
            bin.Width = gameView.DesignResolution.Width / 2;
			fruitBins.Add (bin);
			gameplayLayer.AddChild(bin);
            
         
        }

		private void CreateTouchListener()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            //touchListener.OnTouchesMoved = HandleTouchesMoved;
            touchListener.OnTouchesBegan = HandleTouchesBegan;
			gameplayLayer.AddEventListener (touchListener);
		}

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            if(hasGameEnded)
            {
                var newScene = new TitleScene(GameController.GameView);
                GameController.GoToScene(newScene);
            }
        }

        //some new code
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            //var locationOnScreen = touches[0].Location;
            if (touches.Count > 0)
            {
                //paddle.HandleInput (locationOnScreen);
                if (gravityX > 0)
                {
                    paddleXVerlocity = 0;
                    paddle.PositionX -= 90;

                }
                else
                {
                    paddleXVerlocity = 0;
                    paddle.PositionX += 90;

                }
            }
            //chuyen dong theo chieu doc
            //if (touches.Count > 0)
            //{
            //    paddle.HandleInput (locationOnScreen);
            //    if (gravityY > 0)
            //    {
            //        paddleYVerlocity = 0;
            //        paddle.PositionY -= 100;
            //        paddleYVerlocity = 0;
            //    }
            //    else
            //    {
            //        paddleYVerlocity = 0;
            //        paddle.PositionY += 100;
            //        paddleYVerlocity = 0;

            //    }
            //}
        }
        void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            // we only care about the first touch:
            var locationOnScreen = touches[0].Location;

            paddle.HandleInput(locationOnScreen);
        }

        private void Activity(float frameTimeInSeconds)
		{
            this.frameTimeInSeconds = frameTimeInSeconds;

            if (hasGameEnded == false)
            {

                //new code
                paddleXVerlocity += frameTimeInSeconds * gravityX;
                paddle.PositionX += paddleXVerlocity * frameTimeInSeconds;
                //paddleYVerlocity += frameTimeInSeconds * gravityY;
                //paddle.PositionY += paddleYVerlocity * frameTimeInSeconds;
                
                

                paddle.Activity(frameTimeInSeconds);

                foreach (var fruit in fruitList)
                {
                    fruit.Activity(frameTimeInSeconds);
                    
                    

                }

                spawner.Activity(frameTimeInSeconds);

                DebugActivity();

                PerformCollision();
            }
            //cai nay minh viet hay truong hop ma paddle cham vao khung hinh thi dung lai
            PaddleVsBorders(this.paddle);
        }

        private bool PaddleVsBorders(Paddle paddle)
        {
            if (paddle.PositionX - paddle.Radius < 0)//&& paddleXVerlocity < 0
            {
                paddle.PositionX = 0 + paddle.Radius;
                CCAudioEngine.SharedEngine.PlayEffect("FruitEdgeBounce");
                return true;
            }
            if (paddle.PositionX + paddle.Radius > gameplayLayer.ContentSize.Width)//&& paddleXVerlocity > 0
            {
                paddle.PositionX = gameplayLayer.ContentSize.Width - paddle.Radius;
                CCAudioEngine.SharedEngine.PlayEffect("FruitEdgeBounce");
                return true;
            }

            return false;
        }

        private void DebugActivity()
        {
            if(GameCoefficients.ShowDebugInfo)
            {
                debugLabel.Text = spawner.DebugInfo;
            }
        }

        private void PerformCollision()
		{
            // reverse for loop since fruit may be destroyed:
            for(int i = fruitList.Count - 1; i > -1; i--)
            {
                var fruit = fruitList[i];

                FruitVsPaddle(fruit);

                FruitVsBorders(fruit);

                FruitVsBins(fruit);
            }
        }

        private void FruitVsPaddle(Fruit fruit)
        {
            bool didCollideWithPaddle = FruitPolygonCollision(fruit, paddle.Polygon, paddle.Velocity);
            if (didCollideWithPaddle)
            {
                //bool didAddPoint = fruit.TryAddExtraPointValue();
                //if (didAddPoint)
                //{
                    if (fruit.FruitColor == FruitColor.Red)
                    {
                        //truong hop va vao tuong
                        //score -= 1;
                        //scoreText.Score = score;
                        CCAudioEngine.SharedEngine.PlayEffect("FruitPaddleBounce");
                        //Destroy(fruit);
                    }
                    else if (fruit.FruitColor == FruitColor.Yellow)
                    {
                        //truong hop vuot qua duoc
                        score += 1;
                        scoreText.Score = score;
                        CCAudioEngine.SharedEngine.PlayEffect("FruitPaddleBounce");
                        gravityX *= -1;
                        Destroy(fruit);
                        for (int i = fruitList.Count - 1; i > -1; i--)
                        {
                            var fruitCheckYellow = fruitList[i];

                            if (fruitCheckYellow.FruitColor == FruitColor.Yellow && fruitCheckYellow.PositionY > fruit.PositionY)
                            {
                                Destroy(fruitCheckYellow);
                            }
                        }
                    }

                //}
                

            }


        }

        private void FruitVsBins(Fruit fruit)
        {
            foreach (var bin in fruitBins)
            {
                if (bin.Polygon.CollideAgainst(fruit.Collision))
                {

                    Destroy(fruit);

                }
            }
        }

        private void EndGame()
        {
            hasGameEnded = true;
            spawner.IsSpawning = false;
            paddle.Visible = false;


			// dim the background:
			var drawNode = new CCDrawNode();
			drawNode.DrawRect(
				new CCRect (0,0, 2000, 2000),
				new CCColor4B(0,0,0,160));
			hudLayer.Children.Add(drawNode);


            var endGameLabel = new CCLabel("Game Over\nFinal Score:" + score,
				"Arial", 40, CCLabelFormat.SystemFont);
            endGameLabel.HorizontalAlignment = CCTextAlignment.Center;
			endGameLabel.Color = CCColor3B.White;
            endGameLabel.VerticalAlignment = CCVerticalTextAlignment.Center;
            endGameLabel.PositionX = hudLayer.ContentSize.Width / 2.0f;
            endGameLabel.PositionY = hudLayer.ContentSize.Height / 2.0f;
            hudLayer.Children.Add(endGameLabel);

        }

        private void Destroy(Fruit fruit)
        {
            fruit.RemoveFromParent();
            fruitList.Remove(fruit);
        }

        private static bool FruitPolygonCollision(Fruit fruit, Polygon polygon, CCPoint polygonVelocity)
        {
            // Test whether the fruit collides
            bool didCollide = polygon.CollideAgainst(fruit.Collision);

            if (didCollide)
            {
                var circle = fruit.Collision;

                // Get the separation vector to reposition the fruit so it doesn't overlap the polygon
                var separation = CollisionResponse.GetSeparationVector(circle, polygon);
                fruit.Position += separation;

                // Adjust the fruit's Velocity to make it bounce:
                var normal = separation;
                normal.Normalize();
                fruit.Velocity = CollisionResponse.ApplyBounce(
                    fruit.Velocity, 
                    polygonVelocity, 
                    normal, 
                    GameCoefficients.FruitCollisionElasticity);

            }
            return didCollide;
        }

        private void FruitVsBorders(Fruit fruit)
        {
            if(fruit.PositionX - fruit.Radius < 0 && fruit.Velocity.X < 0)
            {
                fruit.Velocity.X *= -1 * GameCoefficients.FruitCollisionElasticity;
                CCAudioEngine.SharedEngine.PlayEffect("FruitEdgeBounce");
            }
            if(fruit.PositionX + fruit.Radius > gameplayLayer.ContentSize.Width && fruit.Velocity.X > 0)
            {
                fruit.Velocity.X *= -1 * GameCoefficients.FruitCollisionElasticity;
                CCAudioEngine.SharedEngine.PlayEffect("FruitEdgeBounce");

            }
            if (fruit.PositionY + fruit.Radius > gameplayLayer.ContentSize.Height && fruit.Velocity.Y > 0)
            {
                fruit.Velocity.Y *= -1 * GameCoefficients.FruitCollisionElasticity;
                CCAudioEngine.SharedEngine.PlayEffect("FruitEdgeBounce");
            }
        }
	}
}

