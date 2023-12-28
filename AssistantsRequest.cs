using OpenAI.Net.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net
{
    public class AssistantsRequest
    {

        //private AssistantsListResponse assistantsListResponse;
        List<AssistantsRoleCreateResponse> assistantsRoleCreateResponses = new List<AssistantsRoleCreateResponse>();

        //public AssistantsThreadCreateResponse assistantsThreadCreateResponse { get;private set; }
        //public AssistantsRoleCreateResponse assistantsRoleCreateResponse { get; private set; }

        public AssistantsRequest()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public async Task<(AssistantsRoleCreateResponse assistantsRoleCreateResponse, AssistantsThreadCreateResponse assistantsThreadCreateResponse)> InitAsync()
        {
            AssistantsListResponse assistantsListResponse;
            AssistantsRoleCreateResponse assistantsRoleCreateResponse;
            AssistantsThreadCreateResponse assistantsThreadCreateResponse;


            assistantsListResponse = await new AssistantsListRequest().SendMsg();

            if (assistantsListResponse != null && assistantsListResponse.data != null && assistantsListResponse.data.Length > 0)
            {
                assistantsRoleCreateResponse = assistantsListResponse.data[0];

                if (!string.IsNullOrWhiteSpace(assistantsRoleCreateResponse.id))
                    assistantsRoleCreateResponse.Success = true;
                else
                    assistantsRoleCreateResponse = await new AssistantsRoleCreateRequest().SendMsg();
            }
            else
                assistantsRoleCreateResponse = await new AssistantsRoleCreateRequest().SendMsg();

            //assistantsRoleCreateResponses.Add(assistantsRoleCreateResponse);

            assistantsThreadCreateResponse = await new AssistantsThreadCreateRequest().SendMsg();

            return (assistantsRoleCreateResponse, assistantsThreadCreateResponse);

            //if (string.IsNullOrWhiteSpace(threadId))
            //{
            //    assistantsThreadCreateResponse = await new AssistantsThreadCreateRequest().SendMsg();
            //}
            //else
            //{
            //    assistantsThreadCreateResponse = await new AssistantsRetrieveThreadRequest().SendMsg(threadId);

            //    if (assistantsThreadCreateResponse == null || string.IsNullOrWhiteSpace(assistantsThreadCreateResponse.id))
            //        assistantsThreadCreateResponse = await new AssistantsThreadCreateRequest().SendMsg();
            //}
        }
    }
}
