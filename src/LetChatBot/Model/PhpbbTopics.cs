using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbTopics
    {
        public int TopicId { get; set; }
        public int ForumId { get; set; }
        public int IconId { get; set; }
        public sbyte TopicAttachment { get; set; }
        public sbyte TopicApproved { get; set; }
        public sbyte TopicReported { get; set; }
        public string TopicTitle { get; set; }
        public int TopicPoster { get; set; }
        public int TopicTime { get; set; }
        public int TopicTimeLimit { get; set; }
        public int TopicViews { get; set; }
        public int TopicReplies { get; set; }
        public int TopicRepliesReal { get; set; }
        public sbyte TopicStatus { get; set; }
        public sbyte TopicType { get; set; }
        public int TopicFirstPostId { get; set; }
        public string TopicFirstPosterName { get; set; }
        public string TopicFirstPosterColour { get; set; }
        public int TopicLastPostId { get; set; }
        public int TopicLastPosterId { get; set; }
        public string TopicLastPosterName { get; set; }
        public string TopicLastPosterColour { get; set; }
        public string TopicLastPostSubject { get; set; }
        public int TopicLastPostTime { get; set; }
        public int TopicLastViewTime { get; set; }
        public int TopicMovedId { get; set; }
        public sbyte TopicBumped { get; set; }
        public int TopicBumper { get; set; }
        public string PollTitle { get; set; }
        public int PollStart { get; set; }
        public int PollLength { get; set; }
        public sbyte PollMaxOptions { get; set; }
        public int PollLastVote { get; set; }
        public sbyte PollVoteChange { get; set; }
    }
}
