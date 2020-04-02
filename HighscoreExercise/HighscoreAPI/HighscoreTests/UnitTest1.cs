using System;
using Xunit;
using HighscoreAPI;
using HighscoreAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using HighscoreAPI.Model;
using System.Linq;

namespace HighscoreTests
{
    public class UnitTest1
    {
		HighscoreDataContext _context;
		HighscoreController _controller;

		public UnitTest1()
		{
			var options = new DbContextOptionsBuilder<HighscoreDataContext>().UseSqlite("Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 'mydb.db'").Options;
			_context = new HighscoreDataContext(options);
			_controller = new HighscoreController(_context);
		}

		[Fact]
		public async void GetEmptyList()
		{
			_context.HighscoreLists.RemoveRange(_controller.Get());
			await _context.SaveChangesAsync();
			Assert.Equal(0, _context.HighscoreLists.Count());
		}

		[Fact]
		public async void AddHighscore()
		{
			Highscore highscore = new Highscore();
			highscore.User = "KAT";
			highscore.Score = 120;

			Captcha captcha = new Captcha();
			captcha.captcha = "6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE";
			captcha.Highscore = highscore;

			await _controller.PostHighscore(captcha);
			Assert.NotEmpty(_context.HighscoreLists);

			_context.HighscoreLists.RemoveRange(_controller.Get());
			await _context.SaveChangesAsync();
		}


		[Fact]
		public async void GetOrderedList()
		{
			Captcha captcha = new Captcha();
			captcha.captcha = "6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE";

			Highscore highscore1 = new Highscore();
			highscore1.Score = 10;
			highscore1.User = "KAT";
			captcha.Highscore = highscore1;

			await _controller.PostHighscore(captcha);

			Highscore highscore2 = new Highscore();
			highscore2.Score = 200;
			highscore2.User = "KAT";
			captcha.Highscore = highscore2;

			await _controller.PostHighscore(captcha);

			Highscore highscore3 = new Highscore();
			highscore3.Score = 5;
			highscore3.User = "KAT";
			captcha.Highscore = highscore3;

			await _controller.PostHighscore(captcha);

			Assert.Equal(highscore2, _controller.Get().ElementAt(0));

			_context.HighscoreLists.RemoveRange(_controller.Get());
			await _context.SaveChangesAsync();

		}

		[Fact]
		public async void AddToMuchScores()
		{
			Highscore highscore;
			for (var i = 1; i < 15; i++)
			{
				highscore = new Highscore();
				highscore.User = "KAT " + i;
				highscore.Score = 100 + i;

				Captcha captcha = new Captcha();
				captcha.captcha = "6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE";
				captcha.Highscore = highscore;

				await _controller.PostHighscore(captcha);
			}

			Assert.Equal(10, _controller.Get().Count());

			_context.HighscoreLists.RemoveRange(_controller.Get());
			await _context.SaveChangesAsync();
		}

		[Fact]
		public async void AddScoresThenNewHighscore()
		{
			Highscore highscore;
			for (var i = 1; i < 15; i++)
			{
				highscore = new Highscore();
				highscore.User = "player " + i;
				highscore.Score = 100 + i;

				Captcha cap = new Captcha();
				cap.captcha = "6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE";
				cap.Highscore = highscore;
				
				await _controller.PostHighscore(cap);
			}
			highscore = new Highscore();
			highscore.Score = 10000;
			highscore.User = "KAT";

			Captcha captcha = new Captcha();
			captcha.captcha = "6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE";
			captcha.Highscore = highscore;

			await _controller.PostHighscore(captcha);
			Assert.Equal(highscore, _controller.Get().ElementAt(0));

			_context.HighscoreLists.RemoveRange(_controller.Get());
			await _context.SaveChangesAsync();

		}
	}
}
