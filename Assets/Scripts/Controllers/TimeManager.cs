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
    private int _totalTickCount;
    private int _tickCount;
    private int _days;
    private int _cycle;
    
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

    private void Awake()
    {
        IsPaused = true;    
    }

    void Start() {

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

        // Redesign this
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
        _totalTickCount++;

        if (_tickCount >= TICK_TO_DAY){
            //Reset tick count
            _tickCount = 0;
            _days++;
        }

        if(_days >= DAYS_TO_SEASON){
            UpdateSeason();
            _days = 1;
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

    public void LoadTime(int totalTickCount)
    {
        _currentSeason = Season.Spring;

        _totalTickCount = totalTickCount;
        IsPaused = false;

        _elapsedTime = 0.0f;
        _tickCount = totalTickCount % TICK_TO_DAY;

        DoTickMath();
        UpdateClockText();
        UpdateCalendarText();
    }

    private void DoTickMath()
    {
        _days = 1 + (_totalTickCount / TICK_TO_DAY);
        _cycle = 1 + (_days / DAYS_TO_SEASON * 4);
        int seasonInt = _days / DAYS_TO_SEASON;
        _currentSeason = IntToSeason(seasonInt % 4);
        _days = _days % DAYS_TO_SEASON;

    }

    private Season IntToSeason(int season)
    {
        switch (season)
        {
            case 0:
                return Season.Fall;
            case 1:
                return Season.Winter;
            case 2:
                return Season.Spring;
            case 3:
                return Season.Summer;
            default:
                return Season.Fall;
        }
    }

    public int GetTimeToSave()
    {
        return _totalTickCount;
    }

}
