﻿using Management.DomainModels;
using Management.Interface;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainComment = Management.DomainModels.Comment;
using StorageComment = Management.StorageModels.Comment;

namespace Management.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private const string _tableName = "comment";

        private const string _keyColumnName = "uniqueId";
        
        private readonly IRepository _repository;

        public CommentRepository(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<DomainComment>> GetAllCommentsAsync(UserId userId, Location location)
        {
            var result = await _repository.GetSomeAsync<StorageComment>(_tableName, new Dictionary<string, string>()
            {
                { "userId", userId.Value },
                { "country", location.Country.Value },
                { "state", location.State.Value }
            });
            return result.Select(Mapping.StorageToDomainMapper.ToDomain);
        }

        public async Task AddCommentAsync(DomainComment comment)
        {
            var storageComment = Mapping.DomainToStorageMapper.ToStorage(comment);
            // uniqueId, userId, country, state, createdAt, commentStr.
            var fields = new List<string>() { storageComment.UniqueId, storageComment.UserId, storageComment.Country, storageComment.State, storageComment.CreatedAt, storageComment.CommentStr };
            await _repository.InsertAsync<StorageComment>(_tableName, new List<List<string>>() { fields });
        }
    }
}