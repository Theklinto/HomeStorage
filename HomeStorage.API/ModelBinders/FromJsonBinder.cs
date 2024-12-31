using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace HomeStorage.API.ModelBinders
{
    public class FromJsonBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);
            string modelName = bindingContext.ModelName;
            Type modelType = bindingContext.ModelType;

            ValueProviderResult result = bindingContext.ValueProvider.GetValue("json");
            if (result == ValueProviderResult.None || result.FirstValue is null || result.FirstValue.Length == 0)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, result);

            object? parsedJson = JsonConvert.DeserializeObject(result.FirstValue, modelType);
            bool isNullable = Nullable.GetUnderlyingType(modelType) is not null;

            if (parsedJson is null && isNullable is false)
            {
                bindingContext.ModelState.TryAddModelException(modelName, new ArgumentNullException(result.FirstValue));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(parsedJson);
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromJsonAttribute : ModelBinderAttribute<FromJsonBinder> { }
}
