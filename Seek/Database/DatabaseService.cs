﻿
using Microsoft.EntityFrameworkCore;
using Seek.Entities;

namespace Seek.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SeekDbContext _context;
        public DatabaseService(SeekDbContext context)
        {
            _context = context;
        }

        public void Init()
        {
            using (var client = new SeekDbContext())
            {
                client.Database.EnsureCreated();
            }
        }

        public async Task<Setting> GetSettingsAsync(string course)
        {
            return await _context.Settings.AsNoTracking().FirstOrDefaultAsync(_ => _.Course == course);
        }

        public async Task UpdateSettingsAsync(Setting setting)
        {
            var s = await _context.Settings.FirstOrDefaultAsync(_ => _.Course == setting.Course);                       

            if (s != null)
            {
                s.Course = setting.Course;
                s.Chapter = setting.Chapter;
                s.Lesson = setting.Lesson;

                _context.Settings.Update(s);
            }
            else
            {
                setting.Id = Guid.NewGuid();
                await _context.Settings.AddAsync(setting);
            }

            await _context.SaveChangesAsync();
        }
    }
}
