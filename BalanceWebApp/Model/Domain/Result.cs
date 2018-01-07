namespace BalanceWebApp.Model.Domain
{
    public class Result<TL, TR>
    {
        private readonly TR _payload;

        private readonly TL _failure;

        private readonly bool _success;

        private Result(TR payload) {
            _payload = payload;
            _success = true;
        }

        private Result(TL failure) {
            _failure = failure;
            _success = false;
        }

        public TR GetPayload() {
            return _payload;
        }

        public TL GetFailure() {
            return _failure;
        }

        public bool IsSuccess() {
            return _success;
        }

        public bool HasErrors()
        {
            return !IsSuccess();
        }

        public static Result<TL, TR> ForSuccess(TR obj) {
            return new Result<TL, TR>(obj);
        }

        public static Result<TL, TR> ForFailure(TL ex) {
            return new Result<TL, TR>(ex);
        }

    }
}