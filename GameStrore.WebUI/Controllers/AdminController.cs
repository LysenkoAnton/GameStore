﻿using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Games);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        //  Edit() for saving changes
        [HttpPost]
        public ActionResult Edit(Game game, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(game.ImageData, 0, image.ContentLength);
                }
                repository.SaveGame(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
               
                return View(game);
            }
        }



        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deletedGame = repository.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Game \"{0}\" has been deleted",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}