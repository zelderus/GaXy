using System;
using UnityEngine;
using System.Collections;



namespace ZelderFramework.Helpers
{
    /// <summary>
    /// Направление движения.
    /// </summary>
    public enum MoveDirection
    {
        Idle,
        Right,
        Left,
        Up,
        Down
    }
    /// <summary>
    /// Помощь в направлении.
    /// </summary>
    public static class MoveDirectionHelpers
    {
        /// <summary>
        /// Обратное движение.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static MoveDirection GetInversed(MoveDirection direction)
        {
            if (direction == MoveDirection.Idle) return MoveDirection.Idle;

            var inversDir = direction == MoveDirection.Right ? MoveDirection.Left
            : direction == MoveDirection.Left ? MoveDirection.Right
            : direction == MoveDirection.Up ? MoveDirection.Down : MoveDirection.Up;
            return inversDir;
        }
        /// <summary>
        /// Вектор направления.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2 VectorDirection(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Idle: return Vector2.zero;
                case MoveDirection.Up: return new Vector2(0, 1);
                case MoveDirection.Down: return new Vector2(0, -1);
                case MoveDirection.Right: return new Vector2(1, 0);
                case MoveDirection.Left: return new Vector2(-1, 0);
            }
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Действие.
    /// </summary>
    public class GestTouch
    {
        public Int32 FingerId { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 DeltaPosition { get; set; }
        public MoveDirection MoveDirection { get; set; }

        public GestTouch()
        {
            FingerId = -1;
            Position = Vector2.zero;
            DeltaPosition = Vector2.zero;
            MoveDirection = MoveDirection.Idle;
        }
    }


    /// <summary>
    /// Помощь в жестах.
    /// </summary>
    public static class GestHelpers
    {
        #region Hover
        /// <summary>
        /// Позиция на экране.
        /// </summary>
        /// <param name="coordsToLogicScreen">Если необходимо перевести к экрану 768x1280</param>
        public static GestTouch GetHover(Boolean coordsToLogicScreen = false)
        {
            //+ позиция тапа
            Vector3 touchPosition = Vector3.zero;
            Int32 fingerId = -1;

#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8) //&& !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    touchPosition = Input.GetTouch(0).position;
                    fingerId = Input.GetTouch(0).fingerId;
                }
            }
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
            if (Input.GetMouseButton(0))
            {
                touchPosition = Input.mousePosition;
            }
#endif

            //+ TAP
            if (touchPosition != Vector3.zero)
            {
                //+ to normal coords
                touchPosition = coordsToLogicScreen ? DisplayHelper.UnityToScreenCoord(touchPosition) : touchPosition;
                //+ отдаем жест
                var gest = new GestTouch();
                gest.FingerId = fingerId;
                gest.Position = touchPosition;
                return gest;
            }

            return null;
        }
        #endregion

        #region Clap

        public static Boolean GetClap(ref Vector3 pos)
        {
            //+ позиция тапа
            Vector3 touchPosition = Vector3.zero;

            //#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || WINDOWS_PHONE) && !UNITY_EDITOR
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                //if (touch.phase == TouchPhase.Ended)
                {
                    touchPosition = touch.position;
                    pos = touchPosition;
                    return true;
                } 
            } 
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
            if (Input.GetMouseButton(0))
            {
                touchPosition = Input.mousePosition;
                pos = touchPosition;
                return true;
            }
#endif


            return false;
        }

        #endregion

        #region Tap
        /// <summary>
        /// Касание экрана.
        /// </summary>
        /// <param name="coordsToLogicScreen">Если необходимо перевести к экрану 768x1280</param>
        public static GestTouch GetTap(Boolean coordsToLogicScreen = false)
        {
            //+ позиция тапа
            Vector3 touchPosition = Vector3.zero;
            Int32 fingerId = -1;

            //#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || WINDOWS_PHONE) && !UNITY_EDITOR
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    touchPosition = touch.position;
                    fingerId = touch.fingerId;
                } // 
            } 
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
            if (Input.GetMouseButtonUp(0))
            {
                touchPosition = Input.mousePosition;
            }
#endif

            //+ TAP
            if (touchPosition != Vector3.zero)
            {
                //+ to normal coords
                touchPosition = coordsToLogicScreen ? DisplayHelper.UnityToScreenCoord(touchPosition) : touchPosition;
                //+ отдаем жест
                var gest = new GestTouch();
                gest.FingerId = fingerId;
                gest.Position = touchPosition;
                return gest;
            }

            return null;
        }


        public static GestTouch GetTapLog(LogScript log, Boolean coordsToLogicScreen = false)
        {
            //+ позиция тапа
            Vector3 touchPosition = Vector3.zero;
            Int32 fingerId = -1;

            //var touch = Input.GetTouch(0);
            //log.SetText(Input.touchCount.ToString());


            //#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || WINDOWS_PHONE) && !UNITY_EDITOR
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
            //log.SetText(touch.phase.ToString());
                if (touch.phase == TouchPhase.Ended)
                {
                    touchPosition = Input.GetTouch(0).position;
                    fingerId = Input.GetTouch(0).fingerId;
                } // 
            } 
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
            if (Input.GetMouseButtonUp(0))
            {
                touchPosition = Input.mousePosition;
                //log.SetText(touchPosition.ToString());
            }
#endif

            //+ TAP
            if (touchPosition != Vector3.zero)
            {
                //+ to normal coords
                touchPosition = coordsToLogicScreen ? DisplayHelper.UnityToScreenCoord(touchPosition) : touchPosition;
                //+ отдаем жест
                var gest = new GestTouch();
                gest.FingerId = fingerId;
                gest.Position = touchPosition;
                return gest;
            }

            return null;
        }

        #endregion

        #region Swipe
        private static Boolean _inMove = false;
        /// <summary>
        /// Было закончено касание.
        /// </summary>
        /// <returns></returns>
        private static Boolean TouchEnded()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (/*touch.fingerId == fingerId && */ //-
                    touch.phase == TouchPhase.Ended || 
                    touch.phase == TouchPhase.Canceled)// ||
                    //touch.phase == TouchPhase.Stationary)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Свайп. Однократное движение.
        /// </summary>
        /// <param name="coordsToLogicScreen"></param>
        /// <returns></returns>
        public static GestTouch GetSwipe(Boolean coordsToLogicScreen = false)
        {

            //#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8) && !UNITY_EDITOR
#if !UNITY_EDITOR

            //- было закончено прошлое касание - отменяем
            if (_inMove && GestHelpers.TouchEnded())
            {
                _inMove = false;
                return null;
            }
            if (_inMove) return null;
            var move = GestHelpers.GetMove(coordsToLogicScreen);
            if (move != null)
            {
                _inMove = true;
            } // 
            return move;
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
            var move = GestHelpers.GetMove(coordsToLogicScreen);
            return move;
#endif
            return null;
        }


        #endregion

        #region Move

        public static void ClearMove()
        {
            _lastMoveMousePos = new Vector2(-1, -1);
        }
        private static Vector2 _lastMoveMousePos = new Vector2(-1, -1);
        /// <summary>
        /// Движение.
        /// </summary>
        /// <param name="coordsToLogicScreen">Если необходимо перевести к экрану 768x1280</param>
        /// <returns></returns>
        public static GestTouch GetMove(Boolean coordsToLogicScreen = false)
        {
            GestTouch move = null;

            //#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8) && !UNITY_EDITOR
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    move = new GestTouch();
                    move.FingerId = touch.fingerId;
                    move.Position = coordsToLogicScreen ? DisplayHelper.UnityToScreenCoord(touch.position) : touch.position;
                    move.DeltaPosition = touch.deltaPosition;
                    //return gest;
                }

                
            }
#endif



#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC

            if (Input.GetMouseButtonUp(0))
            {
                _lastMoveMousePos = new Vector2(-1, -1);
            }

            if (Input.GetMouseButton(0))
            {
                float dirX = 0;//Input.GetAxis("Mouse X");
                float dirY = 0;//Input.GetAxis("Mouse Y");
                
                if (_lastMoveMousePos.x >= 0 && _lastMoveMousePos.y >= 0)
                {
                    dirX = Input.mousePosition.x - _lastMoveMousePos.x;
                    dirY = Input.mousePosition.y - _lastMoveMousePos.y;
                }
                
                if ((dirX > 0 || dirX < 0) || (dirY > 0 || dirY < 0))
                {
                    move = new GestTouch();
                    move.Position = coordsToLogicScreen ? DisplayHelper.UnityToScreenCoord(Input.mousePosition) : Input.mousePosition;
                    move.DeltaPosition = new Vector2(dirX, dirY);
                    //return gest;
                }

                _lastMoveMousePos = Input.mousePosition;
            }
#endif
            //+ MOVE
            //if (move != null)
            //{
            //    var moveDir = MoveDirection.Idle;
            //    // считаем направление
            //    var mdelta = move.DeltaPosition;
            //    mdelta.Normalize();
            //    if (mdelta.x < 0) moveDir = MoveDirection.Left;
            //    else moveDir = MoveDirection.Right;
            //    var sx = Math.Abs(mdelta.x);
            //    var sy = Math.Abs(mdelta.y);
            //    if (sy > sx)
            //    {
            //        if (mdelta.y < 0) moveDir = MoveDirection.Down;
            //        else moveDir = MoveDirection.Up;
            //    }
            //    move.MoveDirection = moveDir;
            //}



            return move;
        }
        #endregion


    }


}