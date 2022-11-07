using Microsoft.AspNetCore.Mvc;
using ReserveTheBook.WebApi.Models.Reservation;
using ReserveTheBook.WebApi.Services;

namespace ReserveTheBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService) 
        {
            _reservationService = reservationService;
        }

        /// <summary>Get all reservations</summary>
        /// <response code="200">List of reservations</response>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _reservationService.GetAll());

        /// <summary>Get active reservations</summary>
        /// <response code="200">List of reservations</response>
        [HttpGet]
        [Route("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailable() =>
            Ok(await _reservationService.GetActive());

        /// <summary>Get reservation by Id</summary>
        /// <param name="reservationId">reservation Id (Guid)</param>
        /// <response code="200">Return object</response>
        /// <response code="404">Object not found</response>
        [HttpGet]
        [Route("{reservationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid reservationId)
        {
            var reservation = await this._reservationService.GetById(reservationId);
            return reservation is not null ? this.Ok(reservation) : this.NotFound();
        }

        /// <summary>Get reservation by book Id</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <response code="200">Return object</response>
        /// <response code="404">Object not found</response>
        [HttpGet]
        [Route("book/{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByBookId(Guid bookId)
        {
            var reservation = await this._reservationService.GetByBookId(bookId);
            return reservation is not null ? this.Ok(reservation) : this.NotFound();
        }

        /// <summary>Create new reservation</summary>
        /// <param name="request"></param>
        /// <response code="200">Return new object</response>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(OpenReservationRequest request)
            => this.Ok(await this._reservationService.Open(request));

        /// <summary>Update existed reservation</summary>
        /// <param name="reservationId">reservation Id (Guid)</param>
        /// <param name="request"></param>
        /// <response code="200">Return updated object</response>
        /// <response code="404">Object not found</response>
        [HttpPut]
        [Route("update/{reservationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid reservationId, UpdateReservationRequest request)
        {
            var reservation = await this._reservationService.GetById(reservationId);
            return reservation is not null
                ? this.Ok(await this._reservationService.Update(request))
                : this.NotFound();
        }

        /// <summary>Close existed reservation</summary>
        /// <param name="reservationId">reservation Id (Guid)</param>
        /// <param name="request"></param>
        /// <response code="200">Return updated object</response>
        /// <response code="404">Object not found</response>
        [HttpPut]
        [Route("close/{reservationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Close(Guid reservationId, CloseReservationRequest request)
        {
            var reservation = await this._reservationService.GetById(reservationId);
            return reservation is not null
                ? this.Ok(await this._reservationService.Close(request))
                : this.NotFound();
        }

        /// <summary>Delete reservation</summary>
        /// <param name="reservationId">reservation Id (Guid)</param>
        /// <response code="204">Delete succeeded</response>
        /// <response code="404">Object not found</response>
        [HttpDelete]
        [Route("delete/{reservationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid reservationId)
        {
            var reservation = await this._reservationService.GetById(reservationId);
            if (reservation is null)
            {
                return this.NotFound();
            }

            await this._reservationService.Delete(reservationId);

            return this.NoContent();
        }
    }
}
