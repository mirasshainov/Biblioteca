using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace Biblioteca.Models {

 public class Book {
		//Уникальный идентификатор
		public virtual int Id { get; set; }
		//Название
		public virtual string Name { get; set; }
		//Описание
		public virtual string Description { get; set; }
		//Оценка Мира фантастики
		public virtual int MfRaiting { get; set; }
		//Номера страниц
		public virtual int PageNumber { get; set; }
		//Ссылка на картинку
		public virtual string Image { get; set; }
		//Дата поступления книги (фильтр по новинкам!)
		public virtual DateTime IncomeDate { get; set; }
		//Жанр (Многие-ко-Многим)
		//Почему ISet а не IList? Только одна коллекция (IList) может выбираться с помощью JOIN выборки, если нужно  более одной коллекции для выборки JOIN, то лучше их преобразовать в коллекцию ISet
		public virtual ISet<Genre> Genres { get; set; }
		//Серия (Многие-к-одному)
		public virtual Series Series { get; set; }
		//Мнение и другое (Один-к-одному)
		private Mind _mind;
		public virtual Mind Mind {
			get { return _mind ?? (_mind = new Mind()); }
			set { _mind = value; }
		}
		//Автор (Многие-ко-многим)
		public virtual ISet<Author> Authors { get; set; }
		//Заранее инициализируем, чтобы исключение null не возникало.
		public Book() {
			//Неупорядочное множество (в одной таблице не может присутствовать две точь-в-точь одинаковые строки, в противном случае выбирает одну, а другую игнорирует)
			Genres = new HashSet<Genre>();
			Authors = new HashSet<Author>();
		}
	}
	//Маппинг класса Book
	public class BookMap : ClassMap<Book> {
		public BookMap() {
			Id(x => x.Id);
			Map(x => x.Name);
			Map(x => x.Description);
			Map(x => x.MfRaiting);
			Map(x => x.PageNumber);
			Map(x => x.Image);
			Map(x => x.IncomeDate);
			//Отношение многие-ко-многим
			HasManyToMany(x => x.Genres)
			//Правила каскадирования All - Когда объект сохраняется, обновляется или удаляется, проверяются и
			//создаются/обновляются/добавляются все зависимые объекты
			.Cascade.SaveUpdate()
			//Название промежуточной таблицы ДОЛЖНО быть как и у класса Genre!
			.Table("Book_Genre");
			HasManyToMany(x => x.Authors)
		.Cascade.SaveUpdate()
		.Table("Book_Author");
			//Отношение многие к одному
			References(x => x.Series);
			//Отношение один-к-одному. Главный класс.
			HasOne(x => x.Mind).Cascade.All().Constrained();
		}
	}


 public class Author {
		public virtual int Id { get; set; }
		//Имя-Фамилия 
		public virtual string Name { get; set; }
		//Биография
		public virtual string Biography { get; set; }
		//Книжки
		public virtual ISet<Book> Books { get; set; }
		//Инициализация Авторов
		public Author() {
			Books = new HashSet<Book>();
		}
	}
	//Маппинг Автора
	public class AuthorMap : ClassMap<Author> {
		public AuthorMap() {
			Id(x => x.Id);
			Map(x => x.Name);
			Map(x => x.Biography);
			//Отношение многие-ко-многим 
			HasManyToMany(x => x.Books)
			//Правила каскадирования All - Когда объект сохраняется, обновляется или удаляется, проверяются и создаются/обновляются/добавляются все зависимые объекты
			.Cascade.All()
			//Владельцем коллекции явл. другой конец отношения (Book) и он будет сохранен первым.
			.Inverse()
			//Название промежуточной таблицы ДОЛЖНО быть как и у класса Book! 
			.Table("Book_Author");
		}
	}


 public class Genre {
		public virtual int Id { get; set; }
		//Название жанра
		public virtual string Name { get; set; }
		//Английское название жанра
		public virtual string EngName { get; set; }
		//Книжки
		public virtual ISet<Book> Books { get; set; }
		//Инициализация книг
		public Genre() {
			Books = new HashSet<Book>();
		}
	}
	//Маппинг жанра
	public class GenreMap : ClassMap<Genre> {
		public GenreMap() {
			Id(x => x.Id);
			Map(x => x.Name);
			Map(x => x.EngName);
			//Отношение многие-ко-многим
			HasManyToMany(x => x.Books)
			//Правила каскадирования All - Когда объект сохраняется, обновляется или удаляется, проверяются и создаются/обновляются/добавляются все зависимые объекты
			.Cascade.All()
			//Владельцем коллекции явл. другой конец отношения (Book) и он будет сохранен первым.
			.Inverse()
			//Название промежуточной таблицы ДОЛЖНО быть как и у класса Book!
			.Table("Book_Genre");
		}
	}



 public class Mind {
		public virtual int Id { get; set; }
		//Мое мнение
		public virtual string MyMind { get; set; }
		//Мнение фантлаба
		public virtual string MindFantLab { get; set; }
		//Книга
		public virtual Book Book { get; set; }
	}
	//Маппинг Мind
	public class MindMap : ClassMap<Mind> {
		public MindMap() {
			Id(x => x.Id);
			Map(x => x.MyMind);
			Map(x => x.MindFantLab);
			//Отношение один к одному
			HasOne(x => x.Book);
		}
	}


 public class Series {
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }     //Я создал IList, а не ISet, потому что кроме Book, Series больше ни с чем не связана, хотя можно сделать и ISet
		public virtual IList<Book> Books { get; set; }
		//Инициализация книг.
		public Series() {
			Books = new List<Book>();
		}
	}
	public class SeriesMap : ClassMap<Series> {
		public SeriesMap() {
			Id(x => x.Id);
			Map(x => x.Name);
			//Отношение один-ко-многим
			HasMany(x => x.Books)
			////Владельцем коллекции явл. другой конец отношения (Book) и он будет сохранен первым.
			.Inverse();
 
	   }
	}
}