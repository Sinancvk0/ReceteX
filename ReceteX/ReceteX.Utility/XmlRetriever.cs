using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Utility
{
	public class XmlRetriever
	{
		public async Task<string> GetXmlContent(string url)
		{
			using (var client = new HttpClient())
			{
				
				HttpResponseMessage response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsStringAsync();
			}
		}

	}

}

