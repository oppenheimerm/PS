
namespace PA.Core.Helpers
{
	//https://www.softwaretestinghelp.com/csharp-random-number/
	public class RandomStringGenerator
	{
		private readonly Random _random;

		String b = "abcdefghijklmnopqrstuvwxyz0123456789";
		String sc = "!@#=$%^&*~";

		public RandomStringGenerator()
		{
			_random = new Random();
		}

		public string Generate(int length)
		{
			String random = string.Empty;

			for (int i = 0; i < length; i++)
			{
				int a = _random.Next(b.Length); //string.Lenght gets the size of string
				random = random + b.ElementAt(a);
			}
			for (int j = 0; j < 2; j++)
			{
				int sz = _random.Next(sc.Length);
				random = random + sc.ElementAt(sz);
			}

			return random;
		}

	}
}
