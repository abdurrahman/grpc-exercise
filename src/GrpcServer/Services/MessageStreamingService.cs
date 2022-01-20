using System;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcStreaming.Proto;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class MessageStreamingService : StreamService.StreamServiceBase
    {
        private readonly ILogger _logger;

        public MessageStreamingService(ILogger<MessageStreamingService> logger)
        {
            _logger = logger;
        }

        public override async Task StartStreaming(IAsyncStreamReader<StreamMessage> requestStream,
            IServerStreamWriter<StreamMessage> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                if (!string.IsNullOrEmpty(requestStream.Current.Message))
                {
                    if (string.Equals(requestStream.Current.Message, "qw!", StringComparison.OrdinalIgnoreCase))
                        return;
                }

                await Task.Delay(500);
                
                var message = requestStream.Current.Message;
                Console.WriteLine($"Message from client: {requestStream.Current.Name} Message: {message}");
                await responseStream.WriteAsync(new StreamMessage
                {
                    Name = requestStream.Current.Name,
                    Message = $"Reply stream from the server @: {DateTime.Now:s} to {requestStream.Current.Name}",
                });
            }
            
            // if (requestStream != null)
            // {
            //     if (!await requestStream.MoveNext())
            //     {
            //         return;
            //     }
            // }
            //
            // try
            // {
            //     if (!string.IsNullOrEmpty(requestStream.Current.Message))
            //     {
            //         if (string.Equals(requestStream.Current.Message, "qw!", StringComparison.OrdinalIgnoreCase))
            //             return;
            //     }
            //
            //     var message = requestStream.Current.Message;
            //     Console.WriteLine($"Message from client: {requestStream.Current.Name} Message: {message}");
            //     await responseStream.WriteAsync(new StreamMessage
            //     {
            //         Name = requestStream.Current.Name,
            //         Message = $"Reply stream from the server @: {DateTime.Now:s} to {requestStream.Current.Name}",
            //     });
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogInformation(ex.Message);
            // }
        }
    }
}