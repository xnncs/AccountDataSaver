using System.Linq.Expressions;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace AccountDataSaver.Presistence.Extentions;

public static class UpdateAccountProperties
{
    public static SetPropertyCalls<UserAccountEntity> UpdateProperties(
        this SetPropertyCalls<UserAccountEntity> setters, UserAccountModel compareToModel)
    {
        return setters.SetProperty(
                x => x.ServiceUrl,
                x => compareToModel.ServiceUrl)
            .SetProperty(
                x => x.Login,
                x => compareToModel.Login)
            .SetProperty(
                x => x.Description,
                x => compareToModel.Description)
            .SetProperty(
                x => x.Password,
                x => compareToModel.Password);
    }
}