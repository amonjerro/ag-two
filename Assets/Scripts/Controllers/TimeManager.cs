using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    // Main time constants
    const int TICK_TO_DAY = 48;
    const int DAYS_TO_SEASON = 28;
    
    // Should move these out - this is part of the view, should not be in the controller
    public TextMeshProUGUI textClock;
    public TextMeshProUGUI textCalendar;

    // Public properties and time variables
    [SerializeField]
    float tickLength;
    public bool IsPaused { get; set; }

    // Private time variables
    private float _elapsedTime;
    private float _totalElapsedTime;
    private int _totalTickCount;
    private int _tickCount;
    private int _days;
    private int _cycle;
    private Season _currentSeason;
    private Season[] seasonOrder = { Season.Spring, Season.Summer, Season.Fall, Season.Winter };

    public delegate void TickEvent();
    public static event TickEvent Tick;

    // Main delegate event
    public void PublishTick(){
        if (Tick != null){
            Tick();
        }
    }

    // Time controls, should be better bound to player input
    public void SetTickLength(float tickLength){
        this.tickLength = tickLength;
    }


    // Unity lifecycle management
    private void Awake()
    {
        IsPaused = true;    
    }

    void Start() {

    }
    void Update()
    {
        if (IsPaused)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;
        _totalElapsedTime += Time.deltaTime;
        if (_elapsedTime > tickLength)
        {
            _elapsedTime = 0.0f;
            HandleTick();
            UpdateClockText();
            UpdateCalendarText();
            PublishTick();
        }
    }

    // Public methods
    public Season GetCurrentSeason()
    {
        return _currentSeason;
    }

    /// <summary>
    /// Returns the progress of the day expressed as a float between 0-1. Useful for lerping sky color values
    /// </summary>
    /// <returns>Progrss of the day expressed as a float [0-1]</returns>
    public float GetCurrentDayProgress()
    {
        return _totalElapsedTime / (TICK_TO_DAY * tickLength);
    }

    /// <summary>
    /// Sets the time of the clock based on saved data
    /// </summary>
    /// <param name="totalTickCount">The time to load</param>
    public void LoadTime(int totalTickCount)
    {
        _currentSeason = Season.Spring;

        _totalTickCount = totalTickCount;
        IsPaused = false;

        _elapsedTime = 0.0f;
        _totalElapsedTime = _elapsedTime + totalTickCount * tickLength;
        _tickCount = totalTickCount % TICK_TO_DAY;

        DoTickMath();
        UpdateClockText();
        UpdateCalendarText();
        PublishTick();
    }

    /// <summary>
    /// Gets the total amount of ticks the clock has done
    /// </summary>
    /// <returns>The game clock's total tick count</returns>
    public int GetTimeToSave()
    {
        return _totalTickCount;
    }

    // Private methods
    private void UpdateClockText(){
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

    // Handles the logic for calculating day rollover
    private void HandleTick(){
        _tickCount++;
        _totalTickCount++;

        if (_tickCount >= TICK_TO_DAY){
            //Reset tick count
            _tickCount = 0;
            _totalElapsedTime = 0;
            _days++;
        }

        if(_days >= DAYS_TO_SEASON){
            UpdateSeason();
            _days = 1;
        }


    }

    // Handles the logic for calculating season rollover
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

    // Do the math to calculate the status of the clock based on the total tick count
    private void DoTickMath()
    {
        _days = 1 + (_totalTickCount / TICK_TO_DAY);
        _cycle = 1 + (_days / DAYS_TO_SEASON * 4);
        _currentSeason = seasonOrder[(_days / DAYS_TO_SEASON)%4];
        _days = _days % DAYS_TO_SEASON;
    }

}
