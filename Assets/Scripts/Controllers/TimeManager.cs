using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    const int TICK_TO_DAY = 48;

    const int DAYS_TO_SEASON = 28;

    public TextMeshProUGUI textClock;
    public TextMeshProUGUI textCalendar;
    public float tickLength;
    public bool IsPaused { get; set; }

    private float _elapsedTime;

    private int _tickCount;
    private int _days;
    private uint _cycle;
    
    private Season _currentSeason;

    public delegate void TickEvent();
    public static event TickEvent Tick;

    public void PublishTick(){
        if (Tick != null){
            Tick();
        }
    }

    public void SetTickLength(float tickLength){
        this.tickLength = tickLength;
    }

    void Start() {
        _elapsedTime = 0.0f;
        _tickCount = 14;
        _days = 1;
        _cycle = 1;
        _currentSeason = Season.Spring;
        textClock.text = "07:00";
        textCalendar.text = "Day 1, Spring. Year 1";
        IsPaused = true;
    }

    void Update() {
        if (IsPaused)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > tickLength){
            PublishTick();
            _elapsedTime = 0.0f;
            HandleTick();
            UpdateClockText();
            UpdateCalendarText();
        }
        //Normal Time
        if(Input.GetKeyUp(KeyCode.Alpha1)){
            SetTickLength(1.0f);
        }
        //Fast Time
        if(Input.GetKeyUp(KeyCode.Alpha2)){
            SetTickLength(0.5f);
        }

        //Ultra Fast
        if(Input.GetKeyUp(KeyCode.Alpha3)){
            SetTickLength(0.25f);
        }
    }

    void UpdateClockText(){
        string suffix, prefix;
        prefix = (_tickCount / 2).ToString();
        if (_tickCount < 20){
            prefix = "0"+prefix;
        }
        if (_tickCount % 2 == 0){
            suffix = ":00";
        } else {
            suffix = ":30";
        }
        textClock.text = prefix+suffix;
    }

    void UpdateCalendarText(){
        textCalendar.text = string.Format("Day {0}, {1}. Year {2}", _days, _currentSeason.ToString(), _cycle);
    }


    private void HandleTick(){
        _tickCount++;

        if (_tickCount >= TICK_TO_DAY){
            //Reset tick count
            _tickCount = 0;
            _days++;
        }

        if(_days >= DAYS_TO_SEASON){
            UpdateSeason();
            _days = 0;
        }


    }

    private void UpdateSeason(){
        if (_currentSeason == Season.Summer){
            _currentSeason = Season.Fall;
            return;
        }

        if (_currentSeason == Season.Fall){
            _currentSeason = Season.Winter;
            return;
        }

        if (_currentSeason == Season.Winter){
            _currentSeason = Season.Spring;
            _cycle++;
            return;
        }

        if (_currentSeason == Season.Spring){
            _currentSeason = Season.Summer;
            return;
        }
    }


}
