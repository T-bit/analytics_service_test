﻿namespace TestEventService.Services
{
    public interface IEventService
    {
        void TrackEvent(string type, string data);
    }
}