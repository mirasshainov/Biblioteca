using Biblioteca.Models;
using Biblioteca.Models.NHibernate;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
		// GET: Home
		public ActionResult Index() {
			//using(ISession session = NHibernateHelper.OpenSession()) {
			//using(ITransaction transaction = session.BeginTransaction()) {
			//	//Создать, добавить
			//	var createBook = new Book();
			//	createBook.Name = "Metro2033";
			//	createBook.Description = "Постапокалипсическая мистика";
			//	createBook.Authors.Add(new Author { Name = "Глуховский" });
			//	createBook.Genres.Add(new Genre { Name = "Постапокалипсическая мистика" });
			//	createBook.Series = new Series { Name = "Метро" };
			//	createBook.Mind = new Mind { MyMind = "Постапокалипсическая мистика" };
			//	session.SaveOrUpdate(createBook);
			//		//Обновить (По идентификатору)
			//		var series = session.Get<Series>(1);
			//		var updateBook = session.Get<Book>(1);
			//		updateBook.Name = "Metro2034";
			//		updateBook.Description = "Антиутопия";
			//		updateBook.Authors.ElementAt(0).Name = "Глухвский";
			//		updateBook.Genres.ElementAt(0).Name = "Антиутопия";
			//		updateBook.Series = series;
			//		updateBook.Mind.MyMind = "11111";
			//		session.SaveOrUpdate(updateBook);

			//Удаление(По идентификатору)
			//var deleteBook = session.Get<Book>(1);
			//session.Delete(deleteBook);

			//	transaction.Commit();
			//}
			//Genre genreAl = null;
			//Author authorAl = null;
			//Series seriesAl = null;
			//Mind mindAl = null;
			//var books = session.QueryOver<Book>()
			//Left Join с таблицей Genres 
			//.JoinAlias(p => p.Genres, () => genreAl, JoinType.LeftOuterJoin)
			//.JoinAlias(p => p.Authors, () => authorAl, JoinType.LeftOuterJoin)
			//.JoinAlias(p => p.Series, () => seriesAl, JoinType.LeftOuterJoin)
			//.JoinAlias(p => p.Mind, () => mindAl, JoinType.LeftOuterJoin)
			//Убирает повторяющиеся id номера таблицы Book.
			//.TransformUsing(Transformers.DistinctRootEntity).List();
			//return View(books);

			using(ISession session = NHibernateHelper.OpenSession()) {
				Genre genreAl = null;
				Author authorAl = null;
				Series seriesAl = null;
				Mind mindAl = null;
				var books = session.QueryOver<Book>()
					//Left Join с таблицей Genres
					.JoinAlias(p => p.Genres, () => genreAl, JoinType.LeftOuterJoin)
					.JoinAlias(p => p.Authors, () => authorAl, JoinType.LeftOuterJoin)
					.JoinAlias(p => p.Series, () => seriesAl, JoinType.LeftOuterJoin)
					.JoinAlias(p => p.Mind, () => mindAl, JoinType.LeftOuterJoin)
					//Убирает повторяющиеся id номера таблицы Book.
					.TransformUsing(Transformers.DistinctRootEntity).List();
				return View(books);
			}
		}
		}
	}
//}