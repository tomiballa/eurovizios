using System;

namespace Eurovizio
{
    public class Singer
    {
        int year;
        string artist;
        string songTitle;
        int ranking;
        int score;

        public int Year { get => year; set => year = value; }
        public string Artist { get => artist; set => artist = value; }
        public string SongTitle { get => songTitle; set => songTitle = value; }
        public int Ranking { get => ranking; set => ranking = value; }
        public int Score { get => score; set => score = value; }

        public Singer(int year, string artist, string songTitle, int ranking, int score)
        {
            this.year = year;
            this.artist = artist;
            this.songTitle = songTitle;
            this.ranking = ranking;
            this.score = score;
        }
    }
}
