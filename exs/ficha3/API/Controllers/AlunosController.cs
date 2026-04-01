using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AlunosController : Controller
    {
        private readonly HttpClient _client;

        public AlunosController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:4545")
            };
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var response = _client.GetAsync("/api/alunos").Result;
                var content = response.Content.ReadAsStringAsync().Result;

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "Erro ao obter alunos",
                    detalhe = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var response = _client.GetAsync("/api/alunos/" + id).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                return StatusCode((int)response.StatusCode, content);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "Erro ao obter aluno",
                    detalhe = ex.Message
                });
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] Aluno aluno)
        {
            try
            {
                var json = JsonSerializer.Serialize(aluno);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = _client.PostAsync("/api/alunos", httpContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return StatusCode((int)response.StatusCode, content);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "Erro ao criar aluno",
                    detalhe = ex.Message
                });
            }
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromBody] Aluno aluno, int id)
        {

            try
            {
                var json = JsonSerializer.Serialize(aluno);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = _client.PutAsync("/api/alunos/"+id, httpContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return StatusCode((int)response.StatusCode, content);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "Erro ao atualizar aluno",
                    detalhe = ex.Message
                });
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromBody] Aluno aluno, int id)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    "/api/alunos/" + id
                );

                var authHeader = Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(authHeader))
                {
                    request.Headers.Add("Authorization", authHeader);
                }

                var response = _client.SendAsync(request).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                return new ContentResult
                {
                    Content = content,
                    ContentType = "application/json",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "Erro ao eliminar aluno",
                    detalhe = ex.Message
                });
            }
        }
    }
}
