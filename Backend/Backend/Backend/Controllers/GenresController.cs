using BookLibrary.Data.Entities;
using BookLibrary.DTOs;
using BookLibrary.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            var genres = await _genreRepository.GetAllAsync();
            var genreDtos = genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description
            });

            return Ok(genreDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenreDto>> CreateGenre([FromBody] CreateGenreDto createGenreDto)
        {
            var genre = new Genre
            {
                Name = createGenreDto.Name,
                Description = createGenreDto.Description
            };

            await _genreRepository.AddAsync(genre);
            await _genreRepository.SaveAsync();

            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] CreateGenreDto updateGenreDto)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            genre.Name = updateGenreDto.Name;
            genre.Description = updateGenreDto.Description;

            _genreRepository.Update(genre);
            await _genreRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _genreRepository.Delete(genre);
            await _genreRepository.SaveAsync();

            return NoContent();
        }
    }
}