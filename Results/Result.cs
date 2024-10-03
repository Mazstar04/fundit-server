using System.Net;

namespace fundit_server.Results;

  public class Result : BasicActionResult
    {
        public Guid Id { get; set; }

        public Result(Guid id)
        {
            Id = id;
            Status = HttpStatusCode.OK;
        }

        public Result(HttpStatusCode status) : base(status)
        {
        }
    }

