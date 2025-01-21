using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.ViewModels.Base;

namespace Librarian.ViewModels;

public partial class UploadViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string _pdfName = string.Empty;

    [ObservableProperty] 
    private ImageSource _uploadPdfIcon = null!;

    [ObservableProperty] 
    private string _uploadPdfLabel = string.Empty;

    [ObservableProperty] 
    private string _chatTitle = string.Empty;

    public UploadViewModel()
    {
        
    }
    
    [RelayCommand]
    private async Task ChooseFile()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Choose a PDF file to upload.",
                FileTypes = FilePickerFileType.Pdf
            };
            
            var result = await FilePicker.Default.PickAsync(options);
            if (result is null) return;
            await using var stream = await result.OpenReadAsync();
            UploadPdfIcon = ImageSource.FromFile("new_chat_icon");
            UploadPdfLabel = "Start new chat";
            PdfName = result.FileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected override Task OnAppearingAsync()
    {
        UploadPdfIcon = ImageSource.FromFile("attachment_icon");
        UploadPdfLabel = "Upload Pdf";
        PdfName = string.Empty;
        return base.OnAppearingAsync();
    }
}