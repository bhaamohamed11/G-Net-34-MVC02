using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace GymManagementSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);


            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";
            }
            return RedirectToAction(nameof(Create));

        }
        #endregion

        #region MemberDetails
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }
        #endregion
        #region Health Record
        [HttpGet]
        public async Task<IActionResult> GetHealthRecordDetails(int Id, CancellationToken ct)
        {
            var Record = await _memberService.GetMemberHealthRecordAsync(Id, ct);
            if (Record == null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Record);


        }
        #endregion
        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditMember(int Id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(Id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> EditMember(int Id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _memberService.UpdateMemberDetailsAsync(Id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update member. Please try again.";

            return View(model);

        }


        #endregion
        #region delete
        [HttpGet]
        public async Task<IActionResult> Delete(int Id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(Id, ct);
            if (member==null)
            {
                TempData["SuccessMessage"] = "Member deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);




        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(Id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
#endregion