using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Google.Cloud.Translation.V2;


namespace tgBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);
            
            var client = new TelegramBotClient(Environment.GetEnvironmentVariable("tgbotapi")); // api ключ телеграм бота -> Environment Variables надо куда-то прятать его // tgbotapi
            client.StartReceiving(Update, Error);

            
            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
                                   
            if (message.Text != null && message != null)
            {
                if (message.Text.StartsWith("money") && message.Text.Trim().Length >= 9) 
                {
                    string input = message.Text.Substring(6, 3).ToUpper();
                    string[] currencies = new string[]
                    {
                        "AED",
                        "AED",
                        "AFN",
                        "ALL",
                        "AMD",
                        "ANG",
                        "AOA",
                        "ARS",
                        "AUD",
                        "AWG",
                        "AZN",
                        "BAM",
                        "BBD",
                        "BDT",
                        "BGN",
                        "BHD",
                        "BIF",
                        "BMD",
                        "BND",
                        "BOB",
                        "BRL",
                        "BSD",
                        "BTC",
                        "BTN",
                        "BWP",
                        "BYN",
                        "BYR",
                        "BZD",
                        "CAD",
                        "CDF",
                        "CHF",
                        "CLF",
                        "CLP",
                        "CNY",
                        "COP",
                        "CRC",
                        "CUC",
                        "CUP",
                        "CVE",
                        "CZK",
                        "DJF",
                        "DKK",
                        "DOP",
                        "DZD",
                        "EGP",
                        "ERN",
                        "ETB",
                        "EUR",
                        "FJD",
                        "FKP",
                        "GBP",
                        "GEL",
                        "GGP",
                        "GHS",
                        "GIP",
                        "GMD",
                        "GNF",
                        "GTQ",
                        "GYD",
                        "HKD",
                        "HNL",
                        "HRK",
                        "HTG",
                        "HUF",
                        "IDR",
                        "ILS",
                        "IMP",
                        "INR",
                        "IQD",
                        "IRR",
                        "ISK",
                        "JEP",
                        "JMD",
                        "JOD",
                        "JPY",
                        "KES",
                        "KGS",
                        "KHR",
                        "KMF",
                        "KPW",
                        "KRW",
                        "KWD",
                        "KYD",
                        "KZT",
                        "LAK",
                        "LBP",
                        "LKR",
                        "LRD",
                        "LSL",
                        "LTL",
                        "LVL",
                        "LYD",
                        "MAD",
                        "MDL",
                        "MGA",
                        "MKD",
                        "MMK",
                        "MNT",
                        "MOP",
                        "MRO",
                        "MUR",
                        "MVR",
                        "MWK",
                        "MXN",
                        "MYR",
                        "MZN",
                        "NAD",
                        "NGN",
                        "NIO",
                        "NOK",
                        "NPR",
                        "NZD",
                        "OMR",
                        "PAB",
                        "PEN",
                        "PGK",
                        "PHP",
                        "PKR",
                        "PLN",
                        "PYG",
                        "QAR",
                        "RON",
                        "RSD",
                        "RWF",
                        "SAR",
                        "SBD",
                        "SCR",
                        "SDG",
                        "SEK",
                        "SGD",
                        "SHP",
                        "SLE",
                        "SLL",
                        "SOS",
                        "SRD",
                        "SSP",
                        "STD",
                        "SVC",
                        "SYP",
                        "SZL",
                        "THB",
                        "TJS",
                        "TMT",
                        "TND",
                        "TOP",
                        "TRY",
                        "TTD",
                        "TWD",
                        "TZS",
                        "UAH",
                        "UGX",
                        "USD",
                        "UYU",
                        "UZS",
                        "VEF",
                        "VES",
                        "VND",
                        "VUV",
                        "WST",
                        "XAF",
                        "XAG",
                        "XAU",
                        "XCD",
                        "XDR",
                        "XOF",
                        "XPF",
                        "YER",
                        "ZAR",
                        "ZMK",
                        "ZMW",
                        "ZWL"

                    };
                    bool contains = currencies.Contains(input);
                    if (contains)
                    {
                        // API CurrencyLayer
                        string apiKey = Environment.GetEnvironmentVariable("currencyapi"); // currencyapi  // 
                        string url = $"http://api.currencylayer.com/live?access_key={apiKey}&currencies=RUB&source={input}&format=1";

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                // Обработка полученного JSON-ответа и отправка сообщения пользователю
                                // ...
                                //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                                var data = JsonSerializer.Deserialize<JsonElement>(json);

                                // Преобразуем полученные данные в нужные
                                string name = data.GetProperty("quotes").GetProperty($"{input}RUB").GetDecimal().ToString();

                                await botClient.SendTextMessageAsync(message.Chat.Id, $"{input}|RUB: {name}");
                            }
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"ты давай тут не это");
                    }

                }

                if (message.Text.StartsWith("/joke") && message.Text.Trim().Length >= 7) // 0 - 1368 /joke 1  >=7   https://v2.jokeapi.dev/joke/Any?idRange=33
                {
                    bool inputBool = int.TryParse(message.Text.Substring(6, message.Text.Length - 6), out int input);
                    if(inputBool && input >= -319 && input <= 319)
                    {
                        string url = $"https://v2.jokeapi.dev/joke/Any?idRange={input}";
                        TranslationClient translationClient = TranslationClient.CreateFromApiKey(Environment.GetEnvironmentVariable("translateapi")); // translateapi 
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                var data = JsonSerializer.Deserialize<JsonElement>(json);

                                if (data.GetProperty("type").ToString().Equals("twopart"))
                                {
                                    string joke = string.Concat($"{data.GetProperty("setup")}\n", data.GetProperty("delivery"));
                                    string categoryId = string.Concat($"\nCategory: {data.GetProperty("category")}\t | \t", $"Id: {data.GetProperty("id")}");
                                    TranslationResult translationResult = await translationClient.TranslateTextAsync(joke, LanguageCodes.Russian);
                                    string translated = translationResult.TranslatedText;
                                    string finallyJoke = string.Concat($"{joke}\n\n", translated, categoryId);
                                    await botClient.SendTextMessageAsync(message.Chat.Id, finallyJoke);
                                }
                                else
                                {
                                    string joke = data.GetProperty("joke").ToString();
                                    string categoryId = string.Concat($"\nCategory: {data.GetProperty("category")}\t | \t", $"Id: {data.GetProperty("id")}");
                                    TranslationResult translationResult = await translationClient.TranslateTextAsync(joke, LanguageCodes.Russian);
                                    string translated = translationResult.TranslatedText;
                                    string finallyJoke = string.Concat($"{joke}\n\n", translated, categoryId);
                                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{finallyJoke}");

                                }

                            }
                        }
                    }
                    
                }

                Console.WriteLine($"{message.Chat.Username ?? "анон"}\t{message.Text}");

                if (message.Text.ToLower().Contains("йо") || message.Text.ToLower().Contains("yo"))
                {
                    //await botClient.SendTextMessageAsync(message.Chat.Id, "🎲");

                    //await botClient.SendAnimationAsync(chatId, InputFile.FromUri("https://tlgrm.eu/_/stickers/e4d/0d1/e4d0d1cd-3c66-46b7-9ac6-0b897a81320a/192/2.webp")); //

                    await botClient.SendStickerAsync(message.Chat.Id, InputFile.FromFileId("CAACAgQAAxkBAAEKa8plGA8n6Y7khNPYXCNegxsb3VcXzAACfwADS2nuEFTz5nJSOnrDMAQ")); // Я НАШЕЛ ЭТОООО УРААААААААА!!!!!

                    return;
                }
                

                switch (message.Text.ToLower())
                {
                    case "рнд":
                        int res = Variables.rnd.Next(0, 6);
                        await botClient.SendStickerAsync(message.Chat.Id, InputFile.FromFileId(Variables.cube[res]));
                        break;

                    case "rnd":
                        goto case "рнд";
                    case "рол":
                        goto case "рнд";
                    case "rol":
                        goto case "рнд";

                    case "/money_list":
                        await botClient.SendTextMessageAsync(message.Chat.Id, Variables.concurList);
                        break;


                    case "/joke":
                        string url = $"https://v2.jokeapi.dev/joke/Any";
                        TranslationClient translationClient = TranslationClient.CreateFromApiKey(Environment.GetEnvironmentVariable("translateapi"));
                        //string textToTranslate = "how is going on boys?";
                        //string url = $"http://translate.google.ru/translate_a/t?client=x&text=howisgoingguys&hl=en&sl=en&tl=ru";
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                var data = JsonSerializer.Deserialize<JsonElement>(json);
                                

                                // 
                                if (data.GetProperty("type").ToString().Equals("twopart"))
                                {
                                    string joke = string.Concat($"{data.GetProperty("setup")}\n", data.GetProperty("delivery"));
                                    string categoryId = string.Concat($"\nCategory: {data.GetProperty("category")}\t | \t", $"Id: {data.GetProperty("id")}");
                                    TranslationResult translationResult = await translationClient.TranslateTextAsync(joke, LanguageCodes.Russian);
                                    string translated = translationResult.TranslatedText;
                                    string finallyJoke = string.Concat($"{joke}\n\n", translated, categoryId);
                                    await botClient.SendTextMessageAsync(message.Chat.Id, finallyJoke);

                                    //await botClient.SendTextMessageAsync(message.Chat.Id, $"{translated}");
                                    //await botClient.SendTextMessageAsync(message.Chat.Id, joke);
                                }
                                else
                                {
                                    string joke = data.GetProperty("joke").ToString();
                                    string categoryId = string.Concat($"\nCategory: {data.GetProperty("category")}\t | \t", $"Id: {data.GetProperty("id")}");
                                    TranslationResult translationResult = await translationClient.TranslateTextAsync(joke, LanguageCodes.Russian);
                                    string translated = translationResult.TranslatedText;
                                    string finallyJoke = string.Concat($"{joke}\n\n", translated, categoryId);
                                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{finallyJoke}");

                                }
                                // Преобразуем полученные данные в нужные
                                //string name = data.GetProperty("quotes").GetProperty($"{input}RUB").GetDecimal().ToString();

                            }
                        }
                        break;
                    


                    default:
                        //await botClient.SendTextMessageAsync(message.Chat.Id, "не знаю такого :|");
                        break;
                }

            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
    public class Variables
    {
        public static Random rnd = new Random();
        public static string[] cube = new string[] {
                "CAACAgIAAxkBAAEKa8xlGBHedA1vrMFidRK-iqwVgB6xZgACixUAAu-iSEvcMCGEtWaZoDAE",
                "CAACAgIAAxkBAAEKa85lGBH4edjfzy2p6s4OVTMtGprsRAACzxEAAlKRQEtOAAGmnvjK7y8wBA",
                "CAACAgIAAxkBAAEKa9BlGBH6P4K9RnQHSg4q5VFSojHCfgACQBEAAiOsQUurmtw9CutR3zAE",
                "CAACAgIAAxkBAAEKa9JlGBIV-39TLRR744ciGiwIRamCyQACcREAAuzsQUu1GqzW_T-jpDAE",
                "CAACAgIAAxkBAAEKa9RlGBIX9UknnXxe4K7tlafVei2erAACoQ8AAkG1QUtuwcKEzQGhITAE",
                "CAACAgIAAxkBAAEKa9ZlGBIYT21eAAGGM_z588evDyLfT6QAAvYNAAL3rUhLVg8sKkK3KGMwBA"
            };
        public static string concurList = "AED: United Arab Emirates Dirham\nAFN: Afghan Afghani\nALL: Albanian Lek\nAMD: Armenian Dram\nANG: Netherlands Antillean Guilder\nAOA: Angolan Kwanza\nARS: Argentine Peso\nAUD: Australian Dollar\nAWG: Aruban Florin\nAZN: Azerbaijani Manat\nBAM: Bosnia - Herzegovina Convertible Mark\nBBD: Barbadian Dollar\nBDT: Bangladeshi Taka\nBGN: Bulgarian Lev\nBHD: Bahraini Dinar\nBIF: Burundian Franc\nBMD: Bermudan Dollar\nBND: Brunei Dollar\nBOB: Bolivian Boliviano\nBRL: Brazilian Real\nBSD: Bahamian Dollar\nBTC: Bitcoin\nBTN: Bhutanese Ngultrum\nBWP: Botswanan Pula\nBYN: New Belarusian Ruble\nBYR: Belarusian Ruble\nBZD: Belize Dollar\nCAD: Canadian Dollar\nCDF: Congolese Franc\nCHF: Swiss Franc\nCLF: Chilean Unit of Account(UF)\nCLP: Chilean Peso\nCNY: Chinese Yuan\nCOP: Colombian Peso\nCRC: Costa Rican Colón\nCUC: Cuban Convertible Peso\nCUP: Cuban Peso\nCVE: Cape Verdean Escudo\nCZK: Czech Republic Koruna\nDJF: Djiboutian Franc\nDKK: Danish Krone\nDOP: Dominican Peso\nDZD: Algerian Dinar\nEGP: Egyptian Pound\nERN: Eritrean Nakfa\nETB: Ethiopian Birr\nEUR: Euro\nFJD: Fijian Dollar\nFKP: Falkland Islands Pound\nGBP: British Pound Sterling\nGEL: Georgian Lari\nGGP: Guernsey Pound\nGHS: Ghanaian Cedi\nGIP: Gibraltar Pound\nGMD: Gambian Dalasi\nGNF: Guinean Franc\nGTQ: Guatemalan Quetzal\nGYD: Guyanaese Dollar\nHKD: Hong Kong Dollar\nHNL: Honduran Lempira\nHRK: Croatian Kuna\nHTG: Haitian Gourde\nHUF: Hungarian Forint\nIDR: Indonesian Rupiah\nILS: Israeli New Sheqel\nIMP: Manx pound\nINR: Indian Rupee\nIQD: Iraqi Dinar\nIRR: Iranian Rial\nISK: Icelandic Króna\nJEP: Jersey Pound\nJMD: Jamaican Dollar\nJOD: Jordanian Dinar\nJPY: Japanese Yen\nKES: Kenyan Shilling\nKGS: Kyrgystani Som\nKHR: Cambodian Riel\nKMF: Comorian Franc\nKPW: North Korean Won\nKRW: South Korean Won\nKWD: Kuwaiti Dinar\nKYD: Cayman Islands Dollar\nKZT: Kazakhstani Tenge\nLAK: Laotian Kip\nLBP: Lebanese Pound\nLKR: Sri Lankan Rupee\nLRD: Liberian Dollar\nLSL: Lesotho Loti\nLTL: Lithuanian Litas\nLVL: Latvian Lats\nLYD: Libyan Dinar\nMAD: Moroccan Dirham\nMDL: Moldovan Leu\nMGA: Malagasy Ariary\nMKD: Macedonian Denar\nMMK: Myanma Kyat\nMNT: Mongolian Tugrik\nMOP: Macanese Pataca\nMRO: Mauritanian Ouguiya\nMUR: Mauritian Rupee\nMVR: Maldivian Rufiyaa\nMWK: Malawian Kwacha\nMXN: Mexican Peso\nMYR: Malaysian Ringgit\nMZN: Mozambican Metical\nNAD: Namibian Dollar\nNGN: Nigerian Naira\nNIO: Nicaraguan Córdoba\nNOK: Norwegian Krone\nNPR: Nepalese Rupee\nNZD: New Zealand Dollar\nOMR: Omani Rial\nPAB: Panamanian Balboa\nPEN: Peruvian Nuevo Sol\nPGK: Papua New Guinean Kina\nPHP: Philippine Peso\nPKR: Pakistani Rupee\nPLN: Polish Zloty\nPYG: Paraguayan Guarani\nQAR: Qatari Rial\nRON: Romanian Leu\nRSD: Serbian Dinar\nRUB: Russian Ruble (This cannot be used for comparison relative to the ruble) \nRWF: Rwandan Franc\nSAR: Saudi Riyal\nSBD: Solomon Islands Dollar\nSCR: Seychellois Rupee\nSDG: Sudanese Pound\nSEK: Swedish Krona\nSGD: Singapore Dollar\nSHP: Saint Helena Pound\nSLE: Sierra Leonean Leone\nSLL: Sierra Leonean Leone\nSOS: Somali Shilling\nSRD: Surinamese Dollar\nSSP: South Sudanese Pound\nSTD: São Tomé and Príncipe Dobra\nSVC: Salvadoran Colón\nSYP: Syrian Pound\nSZL: Swazi Lilangeni\nTHB: Thai Baht\nTJS: Tajikistani Somoni\nTMT: Turkmenistani Manat\nTND: Tunisian Dinar\nTOP: Tongan Paʻanga\nTRY: Turkish Lira\nTTD: Trinidad and Tobago Dollar\nTWD: New Taiwan Dollar\nTZS: Tanzanian Shilling\nUAH: Ukrainian Hryvnia\nUGX: Ugandan Shilling\nUSD: United States Dollar\nUYU: Uruguayan Peso\nUZS: Uzbekistan Som\nVEF: Venezuelan Bolívar Fuerte\nVES: Sovereign Bolivar\nVND: Vietnamese Dong\nVUV: Vanuatu Vatu\nWST: Samoan Tala\nXAF: CFA Franc BEAC\nXAG: Silver(troy ounce)\nXAU: Gold(troy ounce)\nXCD: East Caribbean Dollar\nXDR: Special Drawing Rights\nXOF: CFA Franc BCEAO\nXPF: CFP Franc\nYER: Yemeni Rial\nZAR: South African Rand\nZMK: Zambian Kwacha(pre-2013)\nZMW: Zambian Kwacha\nZWL: Zimbabwean Dollar";

    }
}
#region
//
//"AED": "United Arab Emirates Dirham",
//"AFN": "Afghan Afghani",
//"ALL": "Albanian Lek",
//"AMD": "Armenian Dram",
//"ANG": "Netherlands Antillean Guilder",
//"AOA": "Angolan Kwanza",
//"ARS": "Argentine Peso",
//"AUD": "Australian Dollar",
//"AWG": "Aruban Florin",
//"AZN": "Azerbaijani Manat",
//"BAM": "Bosnia-Herzegovina Convertible Mark",
//"BBD": "Barbadian Dollar",
//"BDT": "Bangladeshi Taka",
//"BGN": "Bulgarian Lev",
//"BHD": "Bahraini Dinar",
//"BIF": "Burundian Franc",
//"BMD": "Bermudan Dollar",
//"BND": "Brunei Dollar",
//"BOB": "Bolivian Boliviano",
//"BRL": "Brazilian Real",
//"BSD": "Bahamian Dollar",
//"BTC": "Bitcoin",
//"BTN": "Bhutanese Ngultrum",
//"BWP": "Botswanan Pula",
//"BYN": "New Belarusian Ruble",
//"BYR": "Belarusian Ruble",
//"BZD": "Belize Dollar",
//"CAD": "Canadian Dollar",
//"CDF": "Congolese Franc",
//"CHF": "Swiss Franc",
//"CLF": "Chilean Unit of Account (UF)",
//"CLP": "Chilean Peso",
//"CNY": "Chinese Yuan",
//"COP": "Colombian Peso",
//"CRC": "Costa Rican Colón",
//"CUC": "Cuban Convertible Peso",
//"CUP": "Cuban Peso",
//"CVE": "Cape Verdean Escudo",
//"CZK": "Czech Republic Koruna",
//"DJF": "Djiboutian Franc",
//"DKK": "Danish Krone",
//"DOP": "Dominican Peso",
//"DZD": "Algerian Dinar",
//"EGP": "Egyptian Pound",
//"ERN": "Eritrean Nakfa",
//"ETB": "Ethiopian Birr",
//"EUR": "Euro",
//"FJD": "Fijian Dollar",
//"FKP": "Falkland Islands Pound",
//"GBP": "British Pound Sterling",
//"GEL": "Georgian Lari",
//"GGP": "Guernsey Pound",
//"GHS": "Ghanaian Cedi",
//"GIP": "Gibraltar Pound",
//"GMD": "Gambian Dalasi",
//"GNF": "Guinean Franc",
//"GTQ": "Guatemalan Quetzal",
//"GYD": "Guyanaese Dollar",
//"HKD": "Hong Kong Dollar",
//"HNL": "Honduran Lempira",
//"HRK": "Croatian Kuna",
//"HTG": "Haitian Gourde",
//"HUF": "Hungarian Forint",
//"IDR": "Indonesian Rupiah",
//"ILS": "Israeli New Sheqel",
//"IMP": "Manx pound",
//"INR": "Indian Rupee",
//"IQD": "Iraqi Dinar",
//"IRR": "Iranian Rial",
//"ISK": "Icelandic Króna",
//"JEP": "Jersey Pound",
//"JMD": "Jamaican Dollar",
//"JOD": "Jordanian Dinar",
//"JPY": "Japanese Yen",
//"KES": "Kenyan Shilling",
//"KGS": "Kyrgystani Som",
//"KHR": "Cambodian Riel",
//"KMF": "Comorian Franc",
//"KPW": "North Korean Won",
//"KRW": "South Korean Won",
//"KWD": "Kuwaiti Dinar",
//"KYD": "Cayman Islands Dollar",
//"KZT": "Kazakhstani Tenge",
//"LAK": "Laotian Kip",
//"LBP": "Lebanese Pound",
//"LKR": "Sri Lankan Rupee",
//"LRD": "Liberian Dollar",
//"LSL": "Lesotho Loti",
//"LTL": "Lithuanian Litas",
//"LVL": "Latvian Lats",
//"LYD": "Libyan Dinar",
//"MAD": "Moroccan Dirham",
//"MDL": "Moldovan Leu",
//"MGA": "Malagasy Ariary",
//"MKD": "Macedonian Denar",
//"MMK": "Myanma Kyat",
//"MNT": "Mongolian Tugrik",
//"MOP": "Macanese Pataca",
//"MRO": "Mauritanian Ouguiya",
//"MUR": "Mauritian Rupee",
//"MVR": "Maldivian Rufiyaa",
//"MWK": "Malawian Kwacha",
//"MXN": "Mexican Peso",
//"MYR": "Malaysian Ringgit",
//"MZN": "Mozambican Metical",
//"NAD": "Namibian Dollar",
//"NGN": "Nigerian Naira",
//"NIO": "Nicaraguan Córdoba",
//"NOK": "Norwegian Krone",
//"NPR": "Nepalese Rupee",
//"NZD": "New Zealand Dollar",
//"OMR": "Omani Rial",
//"PAB": "Panamanian Balboa",
//"PEN": "Peruvian Nuevo Sol",
//"PGK": "Papua New Guinean Kina",
//"PHP": "Philippine Peso",
//"PKR": "Pakistani Rupee",
//"PLN": "Polish Zloty",
//"PYG": "Paraguayan Guarani",
//"QAR": "Qatari Rial",
//"RON": "Romanian Leu",
//"RSD": "Serbian Dinar",
//"RUB": "Russian Ruble",
//"RWF": "Rwandan Franc",
//"SAR": "Saudi Riyal",
//"SBD": "Solomon Islands Dollar",
//"SCR": "Seychellois Rupee",
//"SDG": "Sudanese Pound",
//"SEK": "Swedish Krona",
//"SGD": "Singapore Dollar",
//"SHP": "Saint Helena Pound",
//"SLE": "Sierra Leonean Leone",
//"SLL": "Sierra Leonean Leone",
//"SOS": "Somali Shilling",
//"SRD": "Surinamese Dollar",
//"SSP": "South Sudanese Pound",
//"STD": "São Tomé and Príncipe Dobra",
//"SVC": "Salvadoran Colón",
//"SYP": "Syrian Pound",
//"SZL": "Swazi Lilangeni",
//"THB": "Thai Baht",
//"TJS": "Tajikistani Somoni",
//"TMT": "Turkmenistani Manat",
//"TND": "Tunisian Dinar",
//"TOP": "Tongan Paʻanga",
//"TRY": "Turkish Lira",
//"TTD": "Trinidad and Tobago Dollar",
//"TWD": "New Taiwan Dollar",
//"TZS": "Tanzanian Shilling",
//"UAH": "Ukrainian Hryvnia",
//"UGX": "Ugandan Shilling",
//"USD": "United States Dollar",
//"UYU": "Uruguayan Peso",
//"UZS": "Uzbekistan Som",
//"VEF": "Venezuelan Bolívar Fuerte",
//"VES": "Sovereign Bolivar",
//"VND": "Vietnamese Dong",
//"VUV": "Vanuatu Vatu",
//"WST": "Samoan Tala",
//"XAF": "CFA Franc BEAC",
//"XAG": "Silver (troy ounce)",
//"XAU": "Gold (troy ounce)",
//"XCD": "East Caribbean Dollar",
//"XDR": "Special Drawing Rights",
//"XOF": "CFA Franc BCEAO",
//"XPF": "CFP Franc",
//"YER": "Yemeni Rial",
//"ZAR": "South African Rand",
//"ZMK": "Zambian Kwacha (pre-2013)",
//"ZMW": "Zambian Kwacha",
//"ZWL": "Zimbabwean Dollar"
#endregion