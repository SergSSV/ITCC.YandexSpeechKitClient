﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Net.Sockets;
using System.Security.Authentication;
using ITCC.YandexSpeeckKitClient.Enums;
using ITCC.YandexSpeeckKitClient.MessageModels.StreamingMode;

namespace ITCC.YandexSpeeckKitClient.Models
{
    /// <summary>
    /// Start speech recognotion session response.
    /// </summary>
    public class StartSessionResult
    {
        /// <summary>
        /// Operation timed out.
        /// </summary>
        public static StartSessionResult TimedOut { get; } = new StartSessionResult { TransportStatus = TransportStatus.Timeout, SocketError = SocketError.TimedOut };

        /// <summary>
        /// Speech recornition session object.
        /// </summary>
        public SpeechRecognitionSession Session { get; }

        /// <summary>
        /// Network-level operation status.
        /// </summary>
        public TransportStatus TransportStatus { get; private set; }

        /// <summary>
        /// The response code.
        /// </summary>
        public ResponseCode ResponseCode { get; }

        /// <summary>
        /// Error message text. Included in the response if the response code is something other than 200.
        /// </summary>
        public string ApiErrorMessage { get; }

        /// <summary>
        /// The session ID. Specify this ID when contacting tech support.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Contains server hello response.
        /// </summary>
        public string ServerHelloResponse { get; }

        /// <summary>
        /// Specifies socker error if TransportStatus = SocketError.
        /// </summary>
        public SocketError SocketError { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string TransportErrorMessage { get; }

        private StartSessionResult()
        {
        }
        internal StartSessionResult(ConnectionResponseMessage connectionResponseMessage, SpeechRecognitionSession session)
        {
            if (connectionResponseMessage == null)
                throw new ArgumentNullException(nameof(connectionResponseMessage));

            TransportStatus = TransportStatus.Ok;
            SocketError = SocketError.Success;
            ResponseCode = connectionResponseMessage.ResponseCode;
            ApiErrorMessage = connectionResponseMessage.Message;
            SessionId = connectionResponseMessage.SessionId;

            if (ResponseCode == ResponseCode.Ok)
                Session = session ?? throw new ArgumentNullException(nameof(session));
        }
        internal StartSessionResult(AuthenticationException authenticationException)
        {
            if (authenticationException == null)
                throw new ArgumentNullException(nameof(authenticationException));

            TransportStatus = TransportStatus.SslNegotiationError;
            SocketError = SocketError.Success;
            TransportErrorMessage = authenticationException.Message;
        }
        internal StartSessionResult(SocketError socketError, string errorMmessage)
        {
            SocketError = socketError;

            if (socketError == SocketError.TimedOut)
            {
                TransportStatus = TransportStatus.Timeout;
                return;
            }

            TransportStatus = TransportStatus.SocketError;
            TransportErrorMessage = errorMmessage;
        }
        internal StartSessionResult(string response)
        {
            TransportStatus = TransportStatus.UnexpectedServerResponse;
            SocketError = SocketError.Success;
            ServerHelloResponse = response;
        }
    }
}
