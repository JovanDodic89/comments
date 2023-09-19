using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Web;

namespace Comments.API.ModelBinders
{
    public class BodyCommandModelBinder : IModelBinder
    {
        private readonly IActionContextAccessor _actionContextAccessor;

        public BodyCommandModelBinder(IActionContextAccessor actionContextAccessor)
        {
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;

            object model = Activator.CreateInstance(modelType);
            
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8))
            {                
                string bodyString = await reader.ReadToEndAsync();

                var body = JsonConvert.DeserializeObject<Dictionary<string, object>>(bodyString);

                foreach (PropertyInfo propertyInfo in modelType.GetProperties())
                {
                    for (int i = 0; i < _actionContextAccessor.ActionContext.RouteData.Values.Keys.Count; i++)
                    {
                        if (_actionContextAccessor.ActionContext.RouteData.Values.Keys.ElementAt(i).ToLower() == propertyInfo.Name.ToLower())
                        {
                            propertyInfo.SetValue(model, Convert.ChangeType(HttpUtility.UrlDecode(_actionContextAccessor.ActionContext.RouteData.Values.Values.ElementAt(i).ToString()),
                                propertyInfo.PropertyType), null);
                            break;
                        }
                    }

                    foreach (var item in body.Keys)
                    {
                        var kebabItem = item.Replace("-", "").ToLower();

                        if (propertyInfo.Name.ToLower() == kebabItem && body[item] != null)
                        {
                            Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                            if (type == typeof(List<string>))
                            {
                                propertyInfo.SetValue(model, JsonConvert.DeserializeObject<List<string>>(body[item].ToString()));
                            }
                            else if (type == typeof(List<int>))
                            {
                                propertyInfo.SetValue(model, JsonConvert.DeserializeObject<List<int>>(body[item].ToString()));
                            }
                            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                            {
                                propertyInfo.SetValue(model, JsonConvert.DeserializeObject(body[item].ToString(), type));
                            }
                            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                propertyInfo.SetValue(model, JsonConvert.DeserializeObject(body[item].ToString(), type));
                            }
                            else if (type.IsClass && type != typeof(string))
                            {
                                propertyInfo.SetValue(model, JsonConvert.DeserializeObject(body[item].ToString(), type));
                            }
                            else if(type.IsEnum)
                            {
                                object value = Enum.Parse(type, body[item].ToString());

                                propertyInfo.SetValue(model, value, null);
                            }
                            else
                            {
                                try
                                {
                                    object safeValue = (body[item] == null) ? null : Convert.ChangeType(body[item], type);

                                    propertyInfo.SetValue(model, safeValue, null);
                                }
                                catch
                                {
                                    propertyInfo.SetValue(model, Activator.CreateInstance(type), null);
                                }   
                            }

                            break;
                        }
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }
}
