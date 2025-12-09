import { useDropzone, FileWithPath } from 'react-dropzone';
import { useCallback } from 'react';

interface UploadDropzoneProps {
  accept: Record<string, string[]>;
  onDrop: (file: File) => void;
  label: string;
  description: string;
  icon: React.ReactNode;
  preview?: string;
  fileName?: string;
  isLoading?: boolean;
}

export const UploadDropzone: React.FC<UploadDropzoneProps> = ({
  accept,
  onDrop,
  label,
  description,
  icon,
  preview,
  fileName,
  isLoading = false,
}) => {
  const onDropCallback = useCallback(
    (acceptedFiles: FileWithPath[]) => {
      if (acceptedFiles[0]) {
        onDrop(acceptedFiles[0]);
      }
    },
    [onDrop]
  );

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    accept,
    maxFiles: 1,
    disabled: isLoading,
    onDrop: onDropCallback,
  });

  return (
    <div>
      <div className="pb-1 text-sm font-medium">{label}</div>
      <div
        {...getRootProps()}
        className={`
          border-2 border-dashed rounded-xl p-8 text-center cursor-pointer
          transition-all duration-200 relative overflow-hidden
          ${isLoading ? 'opacity-50 cursor-not-allowed' : ''}
          ${isDragActive ? 'border-purple-800 bg-purple-900' : 'border-gray-400 hover:border-purple-700'}
          ${fileName ? 'border-purple-800 bg-purple-900' : ''}
        `}
      >
        <input {...getInputProps()} />
        {preview ? (
          <div className="relative w-32 h-32 mx-auto mb-6">
            <img
              src={preview}
              alt="Обложка"
              className="absolute inset-0 w-full h-full object-cover rounded-2xl shadow-2xl"
            />
          </div>
        ) : (
          icon
        )}
        {fileName ? (
          <p className="text-white font-medium">{fileName}</p>
        ) : isDragActive ? (
          <p className="text-white">Отпустите файл сюда...</p>
        ) : (
          <div>
            <p className="text-gray-600">{description}</p>
            <p className="text-sm text-gray-400 mt-2">или нажмите для выбора</p>
          </div>
        )}
      </div>
    </div>
  );
};