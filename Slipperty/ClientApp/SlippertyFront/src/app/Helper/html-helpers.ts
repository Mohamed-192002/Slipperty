import {AppConfigService} from "../Services/app-config.service";



export const LoadingSpinner = `
 <div class="text-center mt-5">
    <div class="spinner-border" style="color:var(--primary-color)" role="status">
        <span class="visually-hidden">Loading...</span>
    </div>
</div>
`;


export  const ErrorDuringLoadingData =  `
  <p class="text-center text-danger">حدث خطأ أثناء تحميل البيانات.</p>
`;


export const NotFounded = `<tr>
    <td colspan="12" class="text-center">
        <img src="${AppConfigService.config.apiUrl}img/404-error.png" style="width: 100%; object-fit: scale-down; height:150px" />
        <h5>لم يتم العثور على اي بيانات</h5>
        <img src="${AppConfigService.config.apiUrl}img/no-results.png" style="width: 100%; object-fit: scale-down; height:300px" />
    </td>
</tr>`
