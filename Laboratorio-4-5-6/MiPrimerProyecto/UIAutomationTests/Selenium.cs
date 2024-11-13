using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using OpenQA.Selenium.Interactions;

namespace UIAutomationTests
{
    [TestFixture]
    internal class SeleniumTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private string baseUrl = "http://localhost:8081/";

        [SetUp]
        public void Setup()
        {
            // Inicializar el driver de Chrome
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Enter_To_List_Of_Countries_Test()
        {
            _driver.Navigate().GoToUrl(baseUrl);

            // Esperar y encontrar el título de la página
            IWebElement pageTitle = _wait.Until(driver => driver.FindElement(By.XPath("//h1[contains(text(), 'Lista de países')]")));

            // Verificar que el título no es nulo
            Assert.That(pageTitle, Is.Not.Null, "El título de la página 'Lista de países' no fue encontrado.");
            Assert.That(pageTitle, Is.Not.Null, "El título de la página 'Lista de países' no fue encontrado.");
        }

        [Test]
        public void Navigate_To_Country_Form_Test()
        {
            _driver.Navigate().GoToUrl(baseUrl);

            // Hacer clic en el botón "Agregar país"
            IWebElement addCountry = _wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/pais']/button[contains(text(), 'Agregar país')]")));
            addCountry.Click();

            // Esperar a que el formulario de creación esté visible
            _wait.Until(driver => driver.FindElement(By.Id("nombre")).Displayed);

            // Verificar que los campos están visibles
            IWebElement nameField = _driver.FindElement(By.Id("nombre"));
            IWebElement continentField = _driver.FindElement(By.Id("continente"));
            IWebElement languageField = _driver.FindElement(By.Id("idioma"));

            // Verificar que los campos están visibles usando Assert.IsTrue
            Assert.That(nameField.Displayed, Is.True, "El campo 'Nombre' no está visible.");
            Assert.That(continentField.Displayed, Is.True, "El campo 'Continente' no está visible.");
            Assert.That(languageField.Displayed, Is.True, "El campo 'Idioma' no está visible.");
        }

        [Test]
        public void Create_New_Country_Test()
        {
            _driver.Navigate().GoToUrl(baseUrl);

            // Hacer clic en el botón "Agregar país"
            IWebElement addCountry = _wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/pais']/button[contains(text(), 'Agregar país')]")));
            addCountry.Click();

            // Esperar a que el formulario de creación esté visible
            _wait.Until(driver => driver.FindElement(By.Id("nombre")).Displayed);

            // Generar un nombre único para el país
            string countryName = "TestCountry" + DateTime.Now.Ticks;
            _driver.FindElement(By.Id("nombre")).SendKeys(countryName);

            // Seleccionar "Europa" en el campo 'Continente'
            IWebElement continenteSelectElement = _driver.FindElement(By.Id("continente"));
            SelectElement continenteSelect = new SelectElement(continenteSelectElement);
            continenteSelect.SelectByText("Europa");

            // Ingresar el idioma
            _driver.FindElement(By.Id("idioma")).SendKeys("TestIdioma");

            // Hacer clic en el botón "Guardar"
            IWebElement guardarButton = _driver.FindElement(By.XPath("//button[contains(text(),'Guardar')]"));
            guardarButton.Click();

            // Esperar a que la URL se redirija a la lista de países
            _wait.Until(driver => driver.Url == baseUrl);

            // Esperar a que la tabla se actualice y el nuevo país aparezca
            bool isCountryPresent = _wait.Until(driver =>
                driver.FindElements(By.XPath($"//td[contains(text(),'{countryName}')]")).Count > 0
            );

            // Verificar que el nuevo país está presente en la lista
            Assert.That(isCountryPresent, Is.Not.Null, "El nuevo país no fue añadido a la lista.");
            Assert.That(isCountryPresent, Is.True, "El nuevo país no fue añadido a la lista.");
        }

        [Test]
        public void Delete_Country_Test()
        {
            _driver.Navigate().GoToUrl(baseUrl);

            // **Paso 1: Crear un nuevo país que se eliminará posteriormente**
            // Hacer clic en el botón "Agregar país"
            IWebElement addCountry = _wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/pais']/button[contains(text(), 'Agregar país')]")));
            addCountry.Click();

            // Esperar a que el formulario de creación esté visible
            _wait.Until(driver => driver.FindElement(By.Id("nombre")).Displayed);

            // Generar un nombre único para el país
            string countryName = "DeleteTestCountry" + DateTime.Now.Ticks;
            _driver.FindElement(By.Id("nombre")).SendKeys(countryName);

            // Seleccionar "Europa" en el campo 'Continente'
            IWebElement continenteSelectElement = _driver.FindElement(By.Id("continente"));
            SelectElement continenteSelect = new SelectElement(continenteSelectElement);
            continenteSelect.SelectByText("Europa");

            // Ingresar el idioma
            _driver.FindElement(By.Id("idioma")).SendKeys("DeleteTestIdioma");

            // Hacer clic en el botón "Guardar"
            IWebElement guardarButton = _driver.FindElement(By.XPath("//button[contains(text(),'Guardar')]"));
            guardarButton.Click();

            // Esperar a que la URL se redirija a la lista de países
            _wait.Until(driver => driver.Url == baseUrl);

            // **Paso 2: Verificar que el país se ha creado correctamente**
            bool isCountryCreated = _wait.Until(driver =>
                driver.FindElements(By.XPath($"//td[contains(text(),'{countryName}')]")).Count > 0
            );

            Assert.That(isCountryCreated, Is.Not.Null, "El país de prueba para eliminación no fue creado correctamente.");
            Assert.That(isCountryCreated, Is.True, "El país de prueba para eliminación no fue creado correctamente.");

            // **Paso 3: Encontrar la fila del país creado**
            // Localizar la fila que contiene el nombre del país
            IWebElement countryRow = _wait.Until(driver => driver.FindElement(By.XPath($"//tr[td[contains(text(),'{countryName}')]]")));

            // **Paso 4: Hacer clic en el botón "Eliminar" correspondiente**
            // Encontrar el botón "Eliminar" dentro de la fila localizada
            IWebElement deleteButton = countryRow.FindElement(By.XPath(".//button[contains(text(), 'Eliminar')]"));

            try
            {
                // Intentar hacer clic en el botón "Eliminar" directamente
                deleteButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Si ocurre un error, intentar desplazarse al elemento y hacer clic nuevamente
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteButton);
                _wait.Until(driver => deleteButton.Displayed && deleteButton.Enabled);
                deleteButton.Click();
            }

            // **Paso 5: Verificar que el país ya no está presente en la lista**
            bool isCountryDeleted = _wait.Until(driver =>
                driver.FindElements(By.XPath($"//td[contains(text(),'{countryName}')]")).Count == 0
            );

            Assert.That(isCountryDeleted, Is.Not.Null, "El país no fue eliminado de la lista correctamente.");
            Assert.That(isCountryDeleted, Is.True, "El país no fue eliminado de la lista correctamente.");
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}
