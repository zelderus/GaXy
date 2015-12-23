using System;
using UnityEngine;
using System.Collections;



namespace ZelderFramework.Animations
{

    /// <summary>
    /// Анимация.
    /// </summary>
    public class EaseAnimations
    {
        /// <summary>
        /// Текущее значение анимации.
        /// </summary>
		public float Value;

        public EaseAnimations(EaseAnimationTypes animationType, float start, float end, float totalTime)
		{
			m_started = false;
			_start = start;
			_end = end;
			_startTotalTime = totalTime;
			Value = _start;
            SetAnimationType(animationType);
			SetAnimationData(start, end, _startTotalTime);
		}

		// Установка новых значений анимации
		public void SetAnimationData(float start, float end, float totalTime)
		{
            m_started = false;
            _start = start;
            _end = end;
            _startTotalTime = totalTime;
            Value = _start;
			// если анимация еще не закончилась, то не реагируем
			//if (this->Active()) return;
			TimeLine = 0.0f;
			TotalTime = totalTime;
			StartVal = start;
			EndVal = end; 
			DifVal = EndVal - StartVal;
			Value = StartVal;
		}
        // Установка нового типа анимации
        public void SetAnimationType(EaseAnimationTypes animationType)
        {
            _animation = EaseAnimationsMath.GetAnimationFunction(animationType);
        }

		//
		public void UpdateTotalTime(float totalTime)
		{
			_startTotalTime = TotalTime = totalTime;
		}
		//
		// Установка функции при завершении анимации
		public void SetOnEndFunction(Action onEndFn)
		{
			_onEndFn = onEndFn;
		}
		// Запуск
		public void Start() 
		{ 
			SetAnimationData(_start, _end, _startTotalTime);
			m_started = true;
		}
		// остановка
        public void Stop(bool withCallback = true) { m_started = false; Value = EndVal; if (withCallback) OnEnd(); }
		// Анимация запущена
		public bool IsStarted() { return m_started; }

		// Обновление
		public virtual void Update(float timeDelta)
		{
			if (!m_started) return;

			TimeLine += timeDelta;
            Value = _animation(TimeLine, StartVal, DifVal, TotalTime);
			//- если закончилась анимация
			if (TimeLine >= TotalTime) 
			{
				Stop();
			}
		}

		protected void OnEnd()
		{
            if (_onEndFn != null)
				_onEndFn();
		}


        private Action _onEndFn;
		//private EasingAnimations::EaseAnimations EaseAnimations;
		private float StartVal;
		private float EndVal;
		private float DifVal;
		private float TimeLine;
		private float TotalTime;

        private float _start;
        private float _end;
        private float _startTotalTime;
        private bool m_started;


        private Func<float, float, float, float, float> _animation;


    }


}