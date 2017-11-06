using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace BinaryClock
{
    public partial class Clock : Form
    {
        // Syncs the clock with the system clock
        private Timer startTimer;
        // Timer to count seconds and update clock every second
        private Timer secondTimer;
        // Timer to count minutes and update clock every minute
        private Timer minuteTimer;
        // Timer to count hours and update clock every hour
        private Timer hourTimer;

        // Stores the seconds that the clock updater reads
        private uint seconds;
        // Stores the minutes that the clock updater reads
        private uint minutes;
        // Stores the hours that the clock updater reads
        private uint hours;

        public Clock()
        {
            this.InitializeComponent();

            this.Load += this.Clock_Load;

            // Seconds Timer
            this.secondTimer = new Timer(1000);

            this.secondTimer.Elapsed += this.SecondTimer_Elapsed;

            // Minutes Timer
            this.minuteTimer = new Timer(60000);

            this.minuteTimer.Elapsed += this.MinuteTimer_Elapsed;

            // Hour Timer
            this.hourTimer = new Timer(3600000);

            this.hourTimer.Elapsed += this.HourTimer_Elapsed;
        }

        private void Clock_Load(object sender, EventArgs e)
        {
            this.startTimer = new Timer(DateTime.Now.Millisecond);

            this.startTimer.Elapsed += this.StartTimer_Elapsed;

            this.startTimer.Start();
            
            // Sync time one unit less to compensate for the handler adding a minute on execution
            this.minutes = (uint)DateTime.Now.Minute - 1;
            this.hours = (uint)DateTime.Now.Hour - 1;

            this.MinuteTimer_Elapsed(this, null);
            this.HourTimer_Elapsed(this, null);
        }

        private void StartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.startTimer.Stop();

            // Sync seconds with the current time
            this.seconds = (uint)DateTime.Now.Second;

            // Begin counting seconds from here
            this.secondTimer.Start();
        }

        private void SecondTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if the time needs to loop back to 0
            if (this.seconds < 59)
            {
                this.seconds++;
            }
            else
            {
                this.seconds = 0;

                if (!this.minuteTimer.Enabled)
                {
                    this.minuteTimer.Start();
                    this.MinuteTimer_Elapsed(this, null);
                }
            }

            // Checks if the time value is a one or two digit long number
            if (this.seconds < 10)
            {
                // Set Tens to off light
                this.tdSecondTens1.Image = this.tdSecondTens2.Image = this.tdSecondTens4.Image = 0;

                // Check if the ones digit requires eight
                if (this.seconds / 8 > 0)
                {
                    this.tdSecondOnes8.Image = 1;

                    // Check whether the clock needs to read eight or nine seconds
                    this.tdSecondOnes1.Image = this.seconds - 8 == 1 ? 1 : 0;

                    // Turn off unneeded lights
                    this.tdSecondOnes2.Image = 0;
                    this.tdSecondOnes4.Image = 0;
                }

                // Check if the ones digit requires four
                else
                {
                    // We already established we don't need eight
                    this.tdSecondOnes8.Image = 0;

                    // Check whether the clock requires four
                    if (this.seconds / 4 > 0)
                    {
                        this.tdSecondOnes4.Image = 1;

                        // Checks which lights need to be on based on time
                        switch (this.seconds - 4)
                        {
                            case 3:
                            {
                                this.tdSecondOnes2.Image = this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondOnes2.Image = 1;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                        }
                    }

                    // Performs the exact same switch statement above except with the four light off
                    else
                    {
                        this.tdSecondOnes4.Image = 0;

                        switch (this.seconds)
                        {
                            case 3:
                            {
                                this.tdSecondOnes2.Image = this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondOnes2.Image = 1;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
            else
            {
                // Splits up the value into the ones and tens digits, 
                // then performs almost the exact same logic as the true
                // case except with added Tens lights
                int secondsTens = int.Parse(this.seconds.ToString().ToCharArray()[0].ToString());
                int secondsOnes = int.Parse(this.seconds.ToString().ToCharArray()[1].ToString());

                if (secondsTens == 0)
                {
                    this.tdSecondTens1.Image = this.tdSecondTens2.Image = this.tdSecondTens4.Image = 0;
                }
                else
                {
                    if (secondsTens / 4 > 0)
                    {
                        this.tdSecondTens4.Image = 1;

                        switch (secondsTens - 4)
                        {
                            case 3:
                            {
                                this.tdSecondTens2.Image = this.tdSecondTens1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondTens2.Image = 1;
                                this.tdSecondTens1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondTens2.Image = 0;
                                this.tdSecondTens1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondTens2.Image = 0;
                                this.tdSecondTens1.Image = 0;
                            }
                                break;
                        }
                    }
                    else
                    {
                        switch (secondsTens)
                        {
                            case 3:
                            {
                                this.tdSecondTens2.Image = this.tdSecondTens1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondTens2.Image = 1;
                                this.tdSecondTens1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondTens2.Image = 0;
                                this.tdSecondTens1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondTens2.Image = 0;
                                this.tdSecondTens1.Image = 0;
                            }
                                break;
                        }
                    }
                }

                if (secondsOnes / 8 > 0)
                {
                    this.tdSecondOnes8.Image = 1;

                    this.tdSecondOnes1.Image = secondsOnes - 8 == 1 ? 1 : 0;

                    this.tdSecondOnes2.Image = 0;
                    this.tdSecondOnes4.Image = 0;
                }

                else
                {
                    this.tdSecondOnes8.Image = 0;

                    if (secondsOnes / 4 > 0)
                    {
                        this.tdSecondOnes4.Image = 1;

                        switch (secondsOnes - 4)
                        {
                            case 3:
                            {
                                this.tdSecondOnes2.Image = this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondOnes2.Image = 1;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                    else
                    {
                        this.tdSecondOnes4.Image = 0;

                        switch (secondsOnes)
                        {
                            case 3:
                            {
                                this.tdSecondOnes2.Image = this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdSecondOnes2.Image = 1;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdSecondOnes2.Image = 0;
                                this.tdSecondOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
        }

        private void MinuteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if the time needs to loop back to 0
            if (this.minutes < 59)
            {
                this.minutes++;
            }
            else
            {
                this.minutes = 0;

                if (!this.hourTimer.Enabled)
                {
                    this.hourTimer.Start();

                    this.HourTimer_Elapsed(this, null);
                }
            }

            // Checks if the time value is a one or two digit long number
            if (this.minutes < 10)
            {
                // Set Tens to off light
                this.tdMinuteTens1.Image = this.tdMinuteTens2.Image = this.tdMinuteTens4.Image = 0;

                // Check if the ones digit requires eight
                if (this.minutes / 8 > 0)
                {
                    this.tdMinuteOnes8.Image = 1;

                    // Check whether the clock needs to read eight or nine minutes
                    this.tdMinuteOnes1.Image = this.minutes - 8 == 1 ? 1 : 0;

                    // Turn off unneeded lights
                    this.tdMinuteOnes2.Image = 0;
                    this.tdMinuteOnes4.Image = 0;
                }

                // Check if the ones digit requires four
                else
                {
                    // We already established we don't need eight
                    this.tdMinuteOnes8.Image = 0;

                    // Check whether the clock requires four
                    if (this.minutes / 4 > 0)
                    {
                        this.tdMinuteOnes4.Image = 1;

                        // Checks which lights need to be on based on time
                        switch (this.minutes - 4)
                        {
                            case 3:
                            {
                                this.tdMinuteOnes2.Image = this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteOnes2.Image = 1;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                        }
                    }

                    // Performs the exact same switch statement above except with the four light off
                    else
                    {
                        this.tdMinuteOnes4.Image = 0;

                        switch (this.minutes)
                        {
                            case 3:
                            {
                                this.tdMinuteOnes2.Image = this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteOnes2.Image = 1;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
            else
            {
                // Splits up the value into the ones and tens digits, 
                // then performs almost the exact same logic as the true
                // case except with added Tens lights
                int minutesTens = int.Parse(this.minutes.ToString().ToCharArray()[0].ToString());
                int minutesOnes = int.Parse(this.minutes.ToString().ToCharArray()[1].ToString());

                if (minutesTens == 0)
                {
                    this.tdMinuteTens1.Image = this.tdMinuteTens2.Image = this.tdMinuteTens4.Image = 0;
                }
                else
                {
                    if (minutesTens / 4 > 0)
                    {
                        this.tdMinuteTens4.Image = 1;

                        switch (minutesTens - 4)
                        {
                            case 3:
                            {
                                this.tdMinuteTens2.Image = this.tdMinuteTens1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteTens2.Image = 1;
                                this.tdMinuteTens1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteTens2.Image = 0;
                                this.tdMinuteTens1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteTens2.Image = 0;
                                this.tdMinuteTens1.Image = 0;
                            }
                                break;
                        }
                    }
                    else
                    {
                        switch (minutesTens)
                        {
                            case 3:
                            {
                                this.tdMinuteTens2.Image = this.tdMinuteTens1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteTens2.Image = 1;
                                this.tdMinuteTens1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteTens2.Image = 0;
                                this.tdMinuteTens1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteTens2.Image = 0;
                                this.tdMinuteTens1.Image = 0;
                            }
                                break;
                        }
                    }
                }

                if (minutesOnes / 8 > 0)
                {
                    this.tdMinuteOnes8.Image = 1;

                    this.tdMinuteOnes1.Image = minutesOnes - 8 == 1 ? 1 : 0;

                    this.tdMinuteOnes2.Image = 0;
                    this.tdMinuteOnes4.Image = 0;
                }

                else
                {
                    this.tdMinuteOnes8.Image = 0;

                    if (minutesOnes / 4 > 0)
                    {
                        this.tdMinuteOnes4.Image = 1;

                        switch (minutesOnes - 4)
                        {
                            case 3:
                            {
                                this.tdMinuteOnes2.Image = this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteOnes2.Image = 1;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                    else
                    {
                        this.tdMinuteOnes4.Image = 0;

                        switch (minutesOnes)
                        {
                            case 3:
                            {
                                this.tdMinuteOnes2.Image = this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdMinuteOnes2.Image = 1;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdMinuteOnes2.Image = 0;
                                this.tdMinuteOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
        }

        private void HourTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if the time needs to loop back to 0
            if (this.hours < 23)
            {
                this.hours++;
            }
            else
            {
                this.hours = 0;
            }

            // Checks if the time value is a one or two digit long number
            if (this.hours < 10)
            {
                // Set Tens to off light
                this.tdHourTens1.Image = this.tdHourTens2.Image = 0;

                // Check if the ones digit requires eight
                if (this.hours / 8 > 0)
                {
                    this.tdHourOnes8.Image = 1;

                    // Check whether the clock needs to read eight or nine hours
                    this.tdHourOnes1.Image = this.hours - 8 == 1 ? 1 : 0;

                    // Turn off unneeded lights
                    this.tdHourOnes2.Image = 0;
                    this.tdHourOnes4.Image = 0;
                }

                // Check if the ones digit requires four
                else
                {
                    // We already established we don't need eight
                    this.tdHourOnes8.Image = 0;

                    // Check whether the clock requires four
                    if (this.hours / 4 > 0)
                    {
                        this.tdHourOnes4.Image = 1;

                        // Checks which lights need to be on based on time
                        switch (this.hours - 4)
                        {
                            case 3:
                            {
                                this.tdHourOnes2.Image = this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdHourOnes2.Image = 1;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                        }
                    }

                    // Performs the exact same switch statement above except with the four light off
                    else
                    {
                        this.tdHourOnes4.Image = 0;

                        switch (this.hours)
                        {
                            case 3:
                            {
                                this.tdHourOnes2.Image = this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdHourOnes2.Image = 1;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
            else
            {
                // Splits up the value into the ones and tens digits, 
                // then performs almost the exact same logic as the true
                // case except with added Tens lights
                int hoursTens = int.Parse(this.hours.ToString().ToCharArray()[0].ToString());
                int hoursOnes = int.Parse(this.hours.ToString().ToCharArray()[1].ToString());

                if (hoursTens == 0)
                {
                    this.tdHourTens1.Image = this.tdHourTens2.Image = 0;
                }
                else
                {
                    switch (hoursTens)
                    {
                        case 2:
                        {
                            this.tdHourTens2.Image = 1;
                            this.tdHourTens1.Image = 0;
                        }
                            break;
                        case 1:
                        {
                            this.tdHourTens2.Image = 0;
                            this.tdHourTens1.Image = 1;
                        }
                            break;
                        case 0:
                        {
                            this.tdHourTens2.Image = 0;
                            this.tdHourTens1.Image = 0;
                        }
                            break;
                    }
                }

                if (hoursOnes / 8 > 0)
                {
                    this.tdHourOnes8.Image = 1;

                    this.tdHourOnes1.Image = hoursOnes - 8 == 1 ? 1 : 0;

                    this.tdHourOnes2.Image = 0;
                    this.tdHourOnes4.Image = 0;
                }

                else
                {
                    this.tdHourOnes8.Image = 0;

                    if (hoursOnes / 4 > 0)
                    {
                        this.tdHourOnes4.Image = 1;

                        switch (hoursOnes - 4)
                        {
                            case 3:
                            {
                                this.tdHourOnes2.Image = this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdHourOnes2.Image = 1;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                    else
                    {
                        this.tdHourOnes4.Image = 0;

                        switch (hoursOnes)
                        {
                            case 3:
                            {
                                this.tdHourOnes2.Image = this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 2:
                            {
                                this.tdHourOnes2.Image = 1;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                            case 1:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 1;
                            }
                                break;
                            case 0:
                            {
                                this.tdHourOnes2.Image = 0;
                                this.tdHourOnes1.Image = 0;
                            }
                                break;
                        }
                    }
                }
            }
        }
    }
}
