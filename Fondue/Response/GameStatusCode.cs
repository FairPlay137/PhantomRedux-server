namespace Fondue.Response
{
    public enum GameStatusCode
    {
        /// <summary>No error has occurred.</summary>
        Success = 0,
        /// <summary>An exception has occurred during processing this command.</summary>
        Err_ReturnException = -1,
        /// <summary>The given session ID has expired.</summary>
        Err_SessionExpired = -2,

        /// <summary>The user on this device does not exist. Possibly user attempted to log in without clearing user data?</summary>
        Err_UserDoesntExist = -101,
        /// <summary>The user on this device has been suspended.</summary>
        Err_UserSuspended = -102,
        /// <summary>The user on this device has been permanently banned/deleted.</summary>
        Err_UserDeleted = -103,
        Err_NoItemExist = -104,
        Err_NoAvatarExist = -105,
        Err_NoSupporterExist = -106,
        Err_UnmatchSupporterLevel = -107,
        
        Err_LimitedItemNum = -111,
        Err_LimitedSupporterNum = -112,
        Err_NoApplePurchaseExist = -113,

        Err_UnmatchMedal = -201,
        Err_UnmatchDarkMedal = -202,
        Err_UnmatchRCoin = -203,
        Err_UnmatchHeart = -204,
        /// <summary>Player does not have enough medals for this operation.</summary>
        Err_NotEnoughMedals = -205,
        /// <summary>Player does not have enough dark medals for this operation.</summary>
        Err_NotEnoughDarkMedals = -206,
        /// <summary>Player does not have enough R Coins for this operation.</summary>
        Err_NotEnoughRCoins = -207,
        Err_NotEnoughHearts = -208,
        Err_NotEnoughTickets = -209,
        Err_UnmatchTicket = -210,
        Err_NoPoint = -211,
        Err_ClosedMarchenoir = -212,

        Err_LockDarkGacha = -301,
        Err_LockTicketGacha = -302,
        Err_NoGacha = -303,

        Err_IncorrectPowerup = -401,

        Err_Episode_LackedExcess = -3002,
        Err_Episode_UnmatchItem = -3011,
        Err_Episode_UnmatchSup = -3012,
        Err_Episode_UnmatchMed = -3013,
        Err_Episode_UnmatchDar = -3014,
        Err_Episode_UnmatchTic = -3015,
        Err_Episode_UnmatchExp = -3016,
        Err_Episode_EmptyData = -3021,
        Err_Episode_RCoinEmpty = -3031,

        Err_Friend_NotFriend = -5008,

        Err_PaymentInvalidReceipt = -8001,
        Err_PaymentEmptyReceipt = -8002,
        Err_PaymentInquiryError = -8003,
        Err_PaymentNonexistentPaymentData = -8004,
        Err_PaymentNonexistentPurchaseData = -8005,
        Err_PaymentExistentPurchaseData = -8006,
        Err_PaymentDuplicateReceipt = -8007,

        Err_RhythmValidStamina = -9001,
        Err_RhythmValidGp = -9002,
        Err_RhythmValidExp = -9003,
        Err_RhythmValidMedal = -9004,
        Err_RhythmValidItem = -9005,
        Err_RhythmValidId = -9006,
        Err_RhythmValidSta = -9007,

        /// <summary>The server returned an empty response.</summary>
        Err_ResponseEmpty = -9995,
        /// <summary>Failed to parse the JSON.</summary>
        Err_JsonAnalysisFailed = -9996,
        /// <summary>The connection to the server timed out.</summary>
        Err_Timeout = -9997,
        /// <summary>An internal server error has occurred.</summary>
        Err_InternalServerError = -9998,
        /// <summary>A miscellaneous error has occurred. (likely an Internet issue)</summary>
        Err_Other = -9999
    }
}
