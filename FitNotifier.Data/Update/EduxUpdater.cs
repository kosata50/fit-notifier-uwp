using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Edux;
using FitNotifier.Data.Services.Edux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Update
{
    public class EduxUpdater
    {
        public UserCredencials User { get; private set; }
        public EduxService Edux { get; private set; }

        public EduxUpdater(UserCredencials user)
        {
            User = user;
        }

        public async Task<bool> UpdateClassification(EduxClassification clasiification)
        {
            if (Edux == null)
                Edux = await GetEdux(User);
            EduxClassification newClassification = await Edux.GetClassification(clasiification.CourseCode);
            clasiification.HTML = newClassification.HTML;
            return false;
        }

        private async Task<EduxService> GetEdux(UserCredencials user)
        {
            EduxService edux = new EduxService(user);
            await edux.Login();
            return edux;
        }
    }
}
