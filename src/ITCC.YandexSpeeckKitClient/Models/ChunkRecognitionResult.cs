﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Net.Sockets;
using ITCC.YandexSpeeckKitClient.Enums;
using ITCC.YandexSpeeckKitClient.MessageModels.StreamingMode;

namespace ITCC.YandexSpeeckKitClient.Models
{
    /// <summary>
    /// Speech recognition result.
    /// </summary>
    public class ChunkRecognitionResult
    {
        /// <summary>
        /// Operation timed out.
        /// </summary>
        public static ChunkRecognitionResult TimedOut { get; } = new ChunkRecognitionResult(SocketError.TimedOut);

        /// <summary>
        /// Network-level operation status.
        /// </summary>
        public TransportStatus TransportStatus { get; }

        /// <summary>
        /// Contains error description if socket erroroccured.
        /// </summary>
        public SocketError SocketError { get; }

        /// <summary>
        /// Code of the server response.
        /// </summary>
        public ResponseCode ResponseCode { get; }

        /// <summary>
        /// End of the utterance (phrase). If true, the recognition result contains the N-best list of speech recognition hypotheses. If false, the server returns intermediate results in the same structure as the final results, but without details for each word, and with just one hypothesis. In other words, the response contains a single utterance.
        /// </summary>
        public bool EndOfUtterance { get; }

        /// <summary>
        /// The number of AddData messages that were combined. A single AddDataResponse is returned for several AddData messages.
        /// </summary>
        public int MergedMessagesCount { get; }

        /// <summary>
        /// A set of hypotheses.
        /// </summary>
        public RecognitionResult Recognition { get; }

        /// <summary>
        /// The result of analyzing the audio signal.
        /// </summary>
        public BiometryResult Biometry { get; }

        internal ChunkRecognitionResult(AddDataResponseMessage addDataResponseMessage)
        {
            if (addDataResponseMessage == null)
                throw new ArgumentNullException(nameof(addDataResponseMessage));

            TransportStatus = TransportStatus.Ok;
            ResponseCode = addDataResponseMessage.ResponseCode;
            SocketError = SocketError.Success;

            if (ResponseCode != ResponseCode.Ok)
                return;

            EndOfUtterance = addDataResponseMessage.EndOfUtterance;
            MergedMessagesCount = addDataResponseMessage.MessagesCount;

            if (addDataResponseMessage.Recognition != null)
                Recognition = new RecognitionResult(addDataResponseMessage.Recognition);

            if (addDataResponseMessage.BioResult != null)
                Biometry = new BiometryResult(addDataResponseMessage.BioResult);
        }
        internal ChunkRecognitionResult(SocketError socketError)
        {
            SocketError = socketError;
            TransportStatus = socketError == SocketError.TimedOut ? TransportStatus.Timeout : TransportStatus.SocketError;
        }
    }
}
