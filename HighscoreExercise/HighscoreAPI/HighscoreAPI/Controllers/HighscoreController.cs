using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HighscoreAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace HighscoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HighscoreController : ControllerBase
    {
        private readonly HighscoreDataContext _context;

        public HighscoreController(HighscoreDataContext context)
        {
            _context = context;
        }

        /*
         * Captcha
         * Websitzeschlüssel: 6Lej2OUUAAAAAKWBrOTPfgy_2Y_5GfICOJ_WEOsE
         * Geheimer Schlüssel: 6Lej2OUUAAAAABm5KJsSz4UNEps61cUGDmX3mchS
         */
        /*public static bool CheckCaptcha(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Lej2OUUAAAAABm5KJsSz4UNEps61cUGDmX3mchS=" + gRecaptchaResponse).Result;
            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
                return false;

            return true;
        }*/

        [HttpGet]
        public IEnumerable<Highscore> Get()
        {
            return _context.HighscoreLists.OrderByDescending(h => h.Score).ToList();   
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Highscore>> GetHighscore(Guid id)
        {
            var highscore = await _context.HighscoreLists.FindAsync(id);

            if (highscore == null)
            {
                return NotFound();
            }

            return highscore;
        }

        [HttpPost]
        public async Task<ActionResult<Highscore>> PostHighscore(Captcha captcha)
        {
            //  if (CheckCaptcha(captcha.captcha))
            //  {
            if (_context.HighscoreLists.Count() >= 10)
            {
                var lastHighscore = _context.HighscoreLists.OrderByDescending(h => h.Score).Last();
                if (lastHighscore.Score < captcha.Highscore.Score)
                {
                    _context.HighscoreLists.Remove(lastHighscore);
                }
                else
                {
                    return BadRequest("This score is not a highscore");
                }
            }

            _context.HighscoreLists.Add(captcha.Highscore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHighScore", new { id = captcha.Highscore.HighScoreId }, captcha.Highscore);
         
          /*    }
           *    else
           *    {
           *        return BadRequest("You are a bot!");
                } */
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Highscore>> DeleteHighscore(Guid id)
        {
            var highscore = await _context.HighscoreLists.FindAsync(id);
            if (highscore == null)
            {
                return NotFound();
            }

            _context.HighscoreLists.Remove(highscore);
            await _context.SaveChangesAsync();

            return highscore;
        }

        private bool HighscoreExists(Guid id)
        {
            return _context.HighscoreLists.Any(e => e.HighScoreId == id);
        }
    }
}
