import {Envelope ,Track } from '@/types';

const getTracks = async (): Promise<Track[]> => {
  const response = await fetch(`http://localhost:8080/api/tracks`);

  const envelope = (await response.json()) as Envelope<Track[]>;

  if (envelope.errors && envelope.errors.length > 0) {
    console.error("API Errors:", envelope.errors);
    return [];
  }

  return (envelope.result as any) || [];
};

export default getTracks;