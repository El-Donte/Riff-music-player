import { useDropzone, FileWithPath } from 'react-dropzone';
import { useCallback, useState } from 'react';

interface UploadDropzoneProps {
  accept: Record<string, string[]>;
  onDrop: (file: File) => void;
  label: string;
  description: string;
  icon: React.ReactNode;
  preview?: string;           // для изображений — data URL или серверный URL
  currentFileUrl?: string;    // ← НОВОЕ: URL текущего файла (аудио или изображение) с сервера
  fileName?: string;          // имя нового выбранного файла
  isLoading?: boolean;
  type?: 'image' | 'audio';   // ← НОВОЕ: тип файла для правильного отображения
}

export const UploadDropzone: React.FC<UploadDropzoneProps> = ({
  accept,
  onDrop,
  label,
  description,
  icon,
  preview,
  currentFileUrl,
  fileName,
  isLoading = false,
  type = 'image',
}) => {

  const [hasNewFile, setHasNewFile] = useState(false);

  const onDropCallback = useCallback(
    (acceptedFiles: FileWithPath[]) => {
      if (acceptedFiles[0]) {
        onDrop(acceptedFiles[0]);
        setHasNewFile(true);
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

  const hasCurrentFile = !hasNewFile && !!currentFileUrl;

  return (
    <div>
      <div className="pb-1 text-sm font-medium">{label}</div>
      <div
        {...getRootProps()}
        className={`
          border-2 border-dashed rounded-xl p-6 text-center cursor-pointer
          transition-all duration-200 relative overflow-hidden
          ${isLoading ? 'opacity-50 cursor-not-allowed' : ''}
          ${isDragActive ? 'border-purple-800 bg-purple-900' : 'border-gray-400 hover:border-purple-700'}
          ${(hasNewFile || hasCurrentFile) ? 'border-purple-800 bg-purple-900' : ''}
        `}
      >
        <input {...getInputProps()} />
        
        {hasNewFile ? (
          <div className="flex flex-col items-center">
            {preview ? (
              <div className="relative w-32 h-32 mx-auto mb-4">
                <img
                  src={preview}
                  alt="Предпросмотр"
                  className="absolute inset-0 w-full h-full object-cover rounded-2xl shadow-2xl"
                />
              </div>
            ): (icon)}
            <p className="text-white font-medium truncate max-w-full">{fileName}</p>
          </div>
        ) : hasCurrentFile ? (
          <div className="flex flex-col items-center">
            {preview ? (
              <div className="relative w-32 h-32 mx-auto mb-4">
                <img
                  src={currentFileUrl}
                  alt="Текущая обложка"
                  className="absolute inset-0 w-full h-full object-cover rounded-2xl shadow-2xl"
                />
              </div>
            ) : type === 'audio' ? (
              <div className="mb-4 w-full max-w-xs">
                <audio controls>
                  <source src={currentFileUrl}/>
                </audio>
                <p className="text-celadon-200 text-sm font-medium">
                  {fileName + ".mp3"}
                </p>
              </div>

            ) : null}
          </div>
        ) : (
          <>
            {icon}
            {isDragActive ? (
              <p className="text-white mt-2">Отпустите файл сюда...</p>
            ) : (
              <div className="mt-2">
                <p className="text-gray-600">{description}</p>
                <p className="text-sm text-gray-400 mt-1">или нажмите для выбора</p>
              </div>
            )}
          </>
        )}
      </div>

      {/* Подсказка */}
      <p className="text-xs text-neutral-500 mt-2">
        {hasCurrentFile
          ? "Оставьте пустым, чтобы сохранить текущий файл"
          : "Файл не выбран"}
      </p>
    </div>
  );
};